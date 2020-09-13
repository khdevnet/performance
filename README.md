# Performance testing
Performance testing is the process of determining the speed, responsiveness and stability of a computer, network, software program or device under a workload.

#### Basic operating system terminology
* **Physical Memory** — The actual physical memory chips in a computer. Only the operating system manages physical memory directly.
* **Virtual Memory** — A logical organization of memory in a given process. Virtual memory size can be larger than physical memory. For example, 32-bit programs have a 4 GB address space, even if the computer itself only has 2 GB of RAM. Windows allows the program to access only 2 GB of that by default, but all 4 GB is possible if the executable is large-address aware. (On 32-bit versions of Windows, large-address aware programs are limited to 3 GB.) As of Windows 8.1 and Server 2012, 64-bit processes have a 128 TB process space, far larger than the 4 TB physical memory limit. Some of the virtual memory may be in RAM while other parts are stored on disk in a paging file. Contiguous blocks of virtual memory may not be contiguous in physical memory. All memory addresses in a process are for the virtual memory.
* **Reserved Memory** — A region of virtual memory address space that has been reserved for the process and thus will not be allocated to a future requester. Reserved memory cannot be used for memory allocation requests because there is nothing backing it—it is just a description of a range of memory addresses.
* **Committed Memory** — A region of memory that has a physical backing store. This can be RAM or disk.
* **Page** — An organizational unit of memory. Blocks of memory are allocated in a page, which is usually a few KB in size.
* **Paging** — The process of transferring pages between regions of virtual memory. The page can move to or from another process (soft paging) or the disk (hard paging). Soft paging can be accomplished very quickly by mapping the existing memory into the current process’s virtual address space. Hard paging involves a relatively slow transfer of data to or from disk. Your program must avoid this at all costs to maintain good performance.
* **Page In** — Transfer a page from another location to the current
* **Page Out** — Transfer a page from the current process to another location, such as disk.
* **Context Switch** — The process of saving and restoring the state of a thread or process. Because there are usually more running threads than available processors, there are often many context switches per second.
* **Kernel Mode** — A mode that allows the OS to modify low-level aspects of the hardware’s state, such as modifying certain registers or enabling/disabling interrupts. Transitioning to Kernel Mode requires an operating system call, and can be quite expensive.
* **User Mode** — An unprivileged mode of executing instructions. There is no ability to modify low-level aspects of the system.

####  The Process category of counters surfaces much of this critical information via counters with instances for each process, including:
* % Privileged Time—Amount of time spent in executing privileged (kernel mode) code.
* % Processor Time—Percentage of a single processor the application is using. If your application is using two logical processor cores at 100% each, then this counter will read 200.
* % User Time—Amount of time spent in executing unprivileged (user mode) code.
* IO Data Bytes/sec—How much I/O your process is doing.
* Page Faults/sec—Total number of page faults in your process. A page fault occurs when a page of memory is missing from the current working set. It is important to realize that this number includes both soft and hard page faults. Soft page faults are innocuous and can be caused by the page being in memory, but outside the current process (such as for shared DLLs). Hard page faults are more serious, indicating data that is on disk but not currently in memory. Unfortunately, you cannot track hard page faults per process with performance counters, but you can see it for the entire system with the Memory\Page Reads/sec counter. You can do some correlation with a process’s total page faults plus the system’s overall page reads (hard faults). You can definitively track a process’s hard faults with ETW tracing with the Windows Kernel/Memory/Hard Fault event.
* Pool Nonpaged Bytes—Typically operating system and driver allocated memory for data structures that cannot be paged out such as operating system objects like threads and mutexes, but also custom data structures.
* Pool Paged Bytes—Also for operating system data structures, but these are allowed to be paged out.
* Private Bytes—Committed virtual memory private to the specific process (not shared with any other processes).
* Virtual Bytes—Allocated memory in the process’s address space, some of which may be backed by the page file, shared with other processes, and memory private to the process.
* Working Set—The amount of virtual memory currently resident in physical memory (usually RAM).
* Working Set-Private—The amount of private bytes currently resident in physical memory.
* Thread Count—The number of threads in the process. This may or may not be equal to the number of .NET threads. See Chapter 4 (Asynchronous Programming) for a discussion of .NET thread-related counters.
There are a few other generally useful categories, depending on your application. You can use PerfMon to explore the specific counters found in these categories.
* IPv4/IPv6—Internet Protocol-related counters for datagrams and fragments.
* Memory—System-wide memory counters such as overall paging, available bytes, committed bytes, and much more.
* Objects—Data about kernel-owned objects such as events, mutexes, processes, threads, semaphores, and sections.
* Processor—Counters for each logical processor in the system.
* System—Context switches, alignment fixes, file operations, process count, threads, and more.
* TCPv4/TCPv6—Data for TCP connections and segment transfers.

#### Performance testing metrics
* Throughput: how many units of information a system processes over a specified time;
* Memory: the working storage space available to a processor or workload;
* Response time, or latency: the amount of time that elapses between a user-entered request and the start of a system's response to that request;
* Bandwidth: the volume of data per second that can move between workloads, usually across a network;
* CPU interrupts per second: the number of hardware interrupts a process receives per second.
* Scalability: how system can scale (vertical/horizontal)
* Reliability: number of errors should be less then number of requests

#### Examples of performance requirements
* The system shall be able to process 100 payment transactions per second in peak load.
* In standard workload, the CPU usage shall be less than 50%, leaving 50% for background jobs.
* Production of a simple report shall take less than 20 seconds for 95% of the cases.
* Scrolling one page up or down in a 200 page document shall take at most 1 second.
#### Resources
* [how-to-write-performance-requirements-with-example](http://www.1202performance.com/atricles/how-to-write-performance-requirements-with-example/)

# Performance optimization
#### Approach
![Performance optimization process](https://github.com/khdevnet/dotnet/blob/master/performance/resources/Performance-approach.png)
* Non-functional requirements
* Performance reflects in architecture
* Develop with real data
* Avoid premature and micro optimization
* Environment - maximum close to production or better to use production
* Test data - real data
* Build Configuration - Release
* Performance benchmarking tools (Load UI, Apache, JMeter)

#### Tools
##### Visual Studio Performance Profiler
* https://msdn.microsoft.com/en-us/library/ms182372.aspx
* https://msdn.microsoft.com/en-us/library/mt210448.aspx
* https://docs.microsoft.com/en-us/visualstudio/profiling/profiling-feature-tour
* https://channel9.msdn.com/Shows/Visual-Studio-Toolbox/Visual-Studio-Toolbox-Performance-Profiling
##### JetBrains dotTrace
* https://www.jetbrains.com/profiler/documentation/documentation.html
##### Redgate ANTS
* http://www.red-gate.com/products/dotnet-development/ants-performance-profiler/


#### BOTTLENECKS IDENTIFICATION

* Sub-Optimal Design
* Resources usage   
  CPU   
  Memory     
  IO: File System   
  IO: Network   
* Tools   
  Task Manager    
  Resource Monitor    
  Performance Monitor   
* Server Monitoring   
  Application Insights    
  New Relic   
  Custom Logging    
  Etc.
  
#### PERFORMANCE. TIPS AND TRICKS
* Logical Problems / Not optimal code
* Data Structure misuse – IEnumerable, List, Array, Dictionary, HashSet
* Cache – cached repositories, etc.
* Concurrent or Asynchronous code – async/await, TPL, Parallel.For
* Use StringBuilder
* Avoid Exceptions
* Use XML Readers instead of LINQ to XML
* Avoid Reflection/dynamic
* Use memoization – if(lookup.TryGetValue(key, out value)) return value; lookup[key] = value.ToLower();
* Override GetHashCode and Equals
* Use **as** operator instead **is**
* Use **ref** or **out** when you need copy structure by reference
* Reduce methods size
* Prefer local variables over fields
* Use static readonly fields and constants
* Use static methods and fields
* Dictonary order, SortedDictionary
* Make classes closed using paramater **sealed**

#### Performance Resources
* https://www.pluralsight.com/courses/measuring-dotnet-performance
* https://www.pluralsight.com/courses/dotnet-performance-optimization-profiling-jetbrains-dottrace
* .Net Performance Testing and Optimization - https://www.red-gate.com/library/net-performance-testing-and-optimization-the-complete-guide
* Pro .NET Performance: Optimize Your C# Applications - http://www.apress.com/us/book/9781430244585
* Writing High-Performance .NET Code - http://www.writinghighperf.net/
* Advanced .NET Debugging - http://advanceddotnetdebugging.com/
* .Net Internals and Advanced Debugging Techniques - book  https://www.amazon.ca/Net-Internals-Advanced-Debugging-Techniques/dp/0321934717  or course https://www.pluralsight.com/courses/dotnet-internals-adv-debug
* Performance Tips - https://msdn.microsoft.com/en-us/library/ms973839.aspx
* 52 Perf Tricks - https://www.red-gate.com/library/52-tips-tricks-to-boost-net-performance 
* Optimization - https://www.dotnetperls.com/optimization


# Memory optimization
#### Tools
##### Visual Studio Managed Memory Debugger
* https://msdn.microsoft.com/en-us/library/d5zhxt22.aspx
* https://blogs.msdn.microsoft.com/devops/2013/10/16/net-memory-analysis-enhancements-in-visual-studio-2013/
* https://channel9.msdn.com/Shows/Visual-Studio-Toolbox/Managed-Memory-Analysis-in-Visual-Studio-2013
##### Visual Studio Diagnostic Tools
* https://docs.microsoft.com/en-us/visualstudio/profiling/memory-usage
* https://channel9.msdn.com/Shows/Visual-Studio-Toolbox/Visual-Studio-2015-Diagnostic-Tools
* JetBrains dotMemory
##### https://www.jetbrains.com/dotmemory/
* Redgate ANTS Memory Profiler
* http://www.red-gate.com/products/dotnet-development/ants-memory-profiler/
* Scitech .NET Memory Profiler
* https://memprofiler.com/
##### Concurrency Visualizer for Visual Studio 2017
* Look how often GC runned.
##### PerfView

#### Memory Tips and Tricks
* Files, Connections, etc. – IDisposable – Dispose(), Close(), using
* Clear events subscriptions – if you see +=, then there must be -=
* Use StringBuilder
* Static fields/properties
* Define collections capacity – var list = new List(products.Length);
* Use structs
* Do not call GC.Collect() explicitly
* Boxing/Unboxing 
* Use WeekReferences, ConditionalWeakTable

#### Resources
* Pluralsight\idisposable-best-practices-csharp-developers
* Pluralsight\making-dotnet-applications-even-faster
* Under the Hood of .NET Memory Management - https://www.amazon.com/Under-Hood-NET-Memory-Management/dp/1906434751

# Database
#### Tools
##### MS SQL Profiler
* https://docs.microsoft.com/en-us/sql/tools/sql-server-profiler/sql-server-profiler
* https://www.red-gate.com/simple-talk/sql/performance/how-to-identify-slow-running-queries-with-sql-profiler/
* https://www.mssqltips.com/sqlservertutorial/272/profiler-and-server-side-traces/

##### MS SQL Management Studio
* https://technet.microsoft.com/en-us/library/ms191227(v=sql.105).aspx
* https://www.mssqltips.com/sqlservertip/2170/more-intuitive-tool-for-reading-sql-server-execution-plans/
* http://sqlmag.com/t-sql/understanding-query-plans

##### Redgate SQL Monitor
* http://www.red-gate.com/products/dba/sql-monitor/

#### Tips and Tricks
* Reduce number of queries
* Use Indexes
* Choose proper transaction isolation level
* Retrieve only needed records - return context.Products.ToList().FirstOrDefault(); 
* Don’t select unneeded columns – SELECT * FROM Products
* Entity Framework – rewrite with stored procedures
* Bulk Operations – use SQLBulkCopy class
* Use Cache
* Use IQueryable
* Do not track changes – AutoDetectChangesEnabled; db.Products.Where(p => p.InStock). AsNoTracking().ToList(); 
* Define length for nvarchar columns
* Seek vs Scan, avoid functions in WHERE
* Estimated vs Actual Query Plan
* Update Statistics
* Parameters Sniffing – local variables, OPTION (RECOMPILE)
* Avoid transactions
* Avoid cursors
* Normalization\Denormalization
* Partitioning

#### Resources
* Pluralsight\sqlserver-sqltrace
* Pluralsight\sqlserver-query-plan-analysis
* https://www.mssqltips.com/sql-server-tip-category/9/performance-tuning/
* http://download.red-gate.com/ebooks/SQL/sql-server-execution-plans.pdf

# Web optimization
#### Resources
* Pluralsight\web-performance
* https://www.udacity.com/course/website-performance-optimization--ud884
* https://developers.google.com/web/fundamentals/performance/
* https://dou.ua/lenta/digests/wpo-digest-0/
* https://www.keycdn.com/blog/website-performance-optimization/
* https://medium.com/airbnb-engineering/performance-tuning-e10ac94916df

### Tools
* Fiddler - http://www.telerik.com/fiddler
* https://www.keycdn.com/blog/website-speed-test-tools/
* http://www.softwaretestinghelp.com/performance-testing-tools-load-testing-tools/







