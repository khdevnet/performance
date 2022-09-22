## MYSQL notes
### Query execution isolation level
* **READ UNCOMMITED** - In the READ UNCOMMITTED isolation level, transactions can view the results of
uncommitted transactions. At this level, many problems can occur unless you
really, really know what you are doing and have a good reason for doing it. This
level is rarely used in practice, because its performance isn’t much better than
the other levels, which have many advantages. Reading uncommitted data is also
known as a dirty read.

* **READ COMMITTED** - The default isolation level for most database systems (but not MySQL!) is READ
COMMITTED. It satisfies the simple definition of isolation used earlier: a transaction
will see only those changes made by transactions that were already committed
when it began, and its changes won’t be visible to others until it has committed.
This level still allows what’s known as a **nonrepeatable read**. This means you can
run the same statement twice and see different data.

* **REPEATABLE READ (default transaction isolation level)** solves the problems that READ UNCOMMITTED allows. It guarantees
that any rows a transaction reads will “look the same” in subsequent reads
within the same transaction, but in theory it still allows another tricky problem:
phantom reads. Simply put, a phantom read can happen when you select some
range of rows, another transaction inserts a new row into the range, and then
you select the same range again; you will then see the new “phantom” row.
InnoDB and Falcon solve the phantom read problem with multiversion concurrency
control, which we explain later in this chapter.
REPEATABLE READ is MySQL’s default transaction isolation level

* **SERIALIZABLE (highest level of isolation)**
The highest level of isolation, SERIALIZABLE, solves the phantom read problem by
forcing transactions to be ordered so that they can’t possibly conflict. In a nutshell,
SERIALIZABLE places a lock on every row it reads. At this level, a lot of timeouts
and lock contention may occur. We’ve rarely seen people use this isolation
level, but your application’s needs may force you to accept the decreased concurrency
in favor of the data stability that results.


## Performance notes
#### use different storage engines for different tables, we can use MyIsam for tables which can be run without transactions [Innodb vs MyIsam](http://www.expertphp.in/article/what-are-the-main-differences-between-innodb-and-myisam#:~:text=InnoDB%20does%20not%20support%20FULLTEXT,is%20suitable%20for%20small%20project.)

#### Use EXPLAIN keyword to see the execution plan for the query
* Check the index usage
* Check rows scanned
#### Use LIMIT 1 clause when retrieving Unique Row
* Helps aggregate functions like MIN or MAX
#### Use LIMIT clause to implement pagination logic
#### Try to Convert <> operator to = operator
* = operator increases chances of index usage
#### Avoid using SELECT *
* Forces full table scan
* Wastes network bandwidth
#### Split big DELETE, UPDATE or INSERT query into multiple smaller queries
#### Use appropriate data types for columns
* Smaller columns are faster for performance
#### MySQL query cache is case and space sensitive
* Use same query case for repeat queries
#### Index columns in the WHERE clause, JOIN, ORDER BY
#### Use UNION ALL instead of UNION if duplicate data is permissible in resultset
#### Table order in INNER JOIN clause does not matter
#### Table order in OUTER Joins matter, do test to find best order
#### Use appropriate data types for columns
#### Use same query to repeated cases it wi be used from query cache
 
## Tune server parameters to better serve your application workload
Azure Database for MySQL allows you to configure server parameters for the values that better serve your workload, check this documentation to learn how. Each application has best practices for those parameter, please review those in order to get the best out of the database server. Some of the parameters that you might need to check is:

#### a- innodb_buffer_pool_size:
The buffer pool is where data and indexes are cached: having it as large as possible will ensure you use memory and not disks for most read operations.

#### b- innodb_file_per_table: 
By default, InnoDB tables are stored in the system tablespace. As an alternative, you can store each InnoDB table in its own data file. This feature is called “file-per-table tablespaces” because each table has its own tablespace data file (.ibd file). learn more about it's advantages here.

#### c- query_cache_size
The amount of memory allocated for caching query results. by default this is turned OFF, read more about this here.

#### d- log_bin
The server logs all statements that change data to the binary log, which is used for replication, If you don't have replication enabled please make sure the you have this parameter turned OFF.

#### e- query_store_capture_mode= NONE
Query store capture can help you identify your top consumers and wit statistics but this also adds overhead in terms of performance as the background process will be be collecting data all the time.

#### f- slow_query_log= OFF
Turning slow query log off will save data collection overhead which contributes to better performance, if you don't need low query log turn off and you can enable it later if needed.

#### g- log_queries_not_using_indexes= OFF
This parameter helps you determine missing indexes, while its awesome to learn about missing indexes but keeping this parameter enabled all the time will drop query performance significantly, rule of thumb here is to use it when you need it. 


## Locks detection
### Flush locks
#### Symptoms
The telltale signs to look for include the following:
* The query state of new queries is “Waiting for table flush.” This may
occur for all new queries or only for queries accessing specific tables.
* More and more connections are created.
* Eventually, new connections fail as MySQL is out of connection. The error received for new connections is ER_CON_COUNT_ERROR: “ERROR 1040 (HY000): Too many connections” when using the classic MySQL protocol (by default port 3306) or “MySQL Error 5011: Could not open session” when using the X Protocol (by default port 33060).
* There is at least one query that has been running later than the oldest request for a flush lock.
* There may be a FLUSH TABLES statement in the process list, but this is not always the case.
* When the FLUSH TABLES statement has waited for lock_wait_timeout, an ER_LOCK_WAIT_TIMEOUT error occurs: ERROR: 1205: Lock wait timeout exceeded; try restarting transaction.
Since the default value for lock_wait_timeout is 365 days, this is only likely to occur if the timeout has been reduced.
* If you connect with the mysql command-line client with a default schema set, the connection may seem to hang before you get to the prompt. The same can happen if you change the default schema with
a connection open.
#### The investigation
The investigation of flush locks requires you to look at the list of queries running on the
instance. Unlike other lock contentions, there are no Performance Schema tables or
InnoDB monitor report that can be used to query for the blocking query directly.
shows an example of the output using the sys.session view.
```sql
SELECT *
FROM sys.session
```

### Record-Level Locks
#### Symptoms
In severe cases, you will get a lock wait timeout or a deadlock error, but in many cases, there may be no direct symptoms. Rather the symptom is that queries are slower than normal. This may range from being a fraction of a second slower to being many seconds slower.
For cases where there is a lock wait timeout, you will see an ER_LOCK_WAIT_TIMEOUT error like the one in the following example: ERROR: 1205: Lock wait timeout exceeded; try restarting transaction

#### The investigation
You can query the data_locks and data_lock_waits tables in the Performance Schema which will show the raw lock data and pending locks, respectively. There is also the sys.innodb_lock_waits view which queries the two tables to find pairs of locks with one being blocked by the other.

```sql
SELECT *
FROM sys.innodb_lock_waits
```

### Dead locks
#### Symptoms
The victim of a deadlock receives an error, and the lock_deadlocks InnoDB metric increments. The error that will be returned to the transaction that InnoDB chooses as the victim is ER_LOCK_DEADLOCK:
ERROR: 1213: Deadlock found when trying to get lock; try restarting transaction The lock_deadlocks metric is very useful to keep an eye on how often deadlocks occur. A convenient way to track the value of lock_deadlocks is to use the sys.metrics 
view:
```sql
SELECT *
FROM sys.metrics
WHERE Variable_name = 'lock_deadlocks'\G
```

#### The investigation
```sql
SELECT *
FROM sys.metrics
WHERE Variable_name = 'lock_deadlocks'
```


The main tool to analyze deadlocks is the section with information about the latest
detected deadlock in the InnoDB monitor output. If you have the innodb_print_all_
deadlocks option enabled (OFF by default), you may also have the deadlock information
from the error log; however, the information is the same, so it does not change the
analysis.
The deadlock information contains four parts describing the deadlock and the result.
The parts are
* When the deadlock occurred.
* Information for the first of the transactions involved in the deadlock.
* Information for the second of the transactions involved in the
deadlock.
* Which of the transactions that was rolled back. This information is not included in the error log when innodb_print_all_deadlocks is enabled.
The numbering of the two transactions is arbitrary, and the main purpose is to be
able to refer to one transaction or the other. The two parts with transaction information
are the most important ones. They include how long the transaction was active, some
statistics about the size of the transactions in terms of locks taken and undo log entries
and similar, the query that was blocking waiting for a lock, and information about the
locks involved in the deadlock.
The lock information is not as easy to interpret as when you use the data_locks and
data_lock_waits tables and the sys.innodb_lock_waits view. However, it is not too
difficult once you have tried to perform the analysis a few times.
```sql
SET GLOBAL innodb_print_all_deadlocks = 'ON';
```
The main tool to analyze deadlocks is the section with information about the latest
detected deadlock in the InnoDB monitor output. If you have the innodb_print_all_
deadlocks option enabled (OFF by default), you may also have the deadlock information
from the error log; however, the information is the same, so it does not change the
analysis.

```sql
SHOW ENGINE INNODB STATUS
```

# MySql manual autoincrement thread safe
Locking reads - If you query data and then insert or update related data within the same transaction, the regular SELECT statement does not give enough protection. Other transactions can update or delete the same rows you just queried.
Possible to use SELECT FOR UPDATE command to lock row

[Locking reads](https://dev.mysql.com/doc/refman/8.0/en/innodb-locking-reads.html)

User defined variable is thread safe per user connection
[User variables](https://dev.mysql.com/doc/refman/8.0/en/user-variables.html)

It is possible to use 
```SQL
UPDATE child_codes SET counter_field = LAST_INSERT_ID(counter_field + 1);
SET @id = (SELECT LAST_INSERT_ID());
use Id between multiple tables 
INSERT INTO @Id
INSERT INTO @Id

```

## Resource
[MySQL 8 Query Performance Tuning]()
[azure-database-for-mysql-performance-troubleshooting](https://techcommunity.microsoft.com/t5/azure-database-for-mysql/azure-database-for-mysql-performance-troubleshooting-basics/ba-p/782815)
