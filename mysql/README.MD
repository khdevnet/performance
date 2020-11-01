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


### Performance notes
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
 
### Tune server parameters to better serve your application workload
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
