# CPU Caches
#### Theory
Processors have caches that reduce the memory overhead
Multiple cache levels (L1, L2, L3 and sometimes L4 on modern processors)
Performance difference between L1, L2 and L3 caches     
L1 cache access latency: 4 cycles       
L2 cache access latency: 11 cycles      
L3 cache access latency: 39 cycles      
Main memory access latency: 107 cycles      
It's bad behaviour when cache data stored in several CPU and then when on CPU cache was updated,
we need to reload cache from shared cache (L3) or RAM.

![Cpu caches](https://github.com/khdevnet/dotnet/blob/master/performance/cpu/docs/cpu-caches.png)

#### Resources
* [How L1 and L2 CPU Caches Work](https://www.extremetech.com/extreme/188776-how-l1-and-l2-cpu-caches-work-and-why-theyre-an-essential-part-of-modern-chips)
* [Making .NET Applications Even Faster](https://www.pluralsight.com/courses/making-dotnet-applications-even-faster)

# CPU Vectorization
#### Theory
**Vectorization**, then, is the process of using these vector registers, instead of scalar registers, in an attempt to make the program run faster. In a perfect world, our example loop would execute 4 times faster.    

Chips have included an additional set of 16 registers, each 128 bits wide. Rather than hold a single, 128-bit wide value, they can hold a collection of smaller values; for example, 4 integers, each 32 bits wide. These registers are called XMM0, XMM1 and so on. They are called **vector registers** because they can hold several values at any one time. The chips also provide new instructions to perform arithmetic on these 4 packed integers. So we can **add 4 pairs of integers with a single instruction**, in the same time that it took to add just **1 pair of integers** when using the **scalar registers**. [These new instructions are called SSE, which stands for “Streaming SIMD Extensions”]

**SIMD** (Single Instruction Multiple Data) operations can be parallelized at the hardware level. That increases the throughput of the vectorized computations, which are common in mathematical, scientific, and graphics apps.
**SIMD** instructions allow multiple calculations to be carried out simultaneously on a single core by using a register that is multiple times bigger than the data being processed.

![Vectorization calculations](https://github.com/khdevnet/dotnet/blob/master/performance/cpu/docs/vector-add-registers.jpg)

#### Tips and Tricks
* Use types from [System.Numerics](https://docs.microsoft.com/en-us/dotnet/api/system.numerics?view=netframework-4.7.2) Namespace
* Manually, by you, the programmer. You need to write in assembly language, or call built-in functions (called “intrinsics”), from your C++ program. This gives you low-level control, but is difficult. You don’t want to be doing this.
* Automatically, on your behalf, by the compiler. This relies upon the compiler being smart enough to recognize loops that can be safely vectorized, and doing so.(C++ compilers mostly not c#)

#### Resources
* [What is Vectorization](https://blogs.msdn.microsoft.com/nativeconcurrency/2012/04/12/what-is-vectorization/)
* [Automatic vectorization](https://en.wikipedia.org/wiki/Automatic_vectorization#Techniques)
* [Making .NET Applications Even Faster](https://www.pluralsight.com/courses/making-dotnet-applications-even-faster)


# CPU Pipelines
#### Theory
**CPU/Processor pipeline** is a series of instructions that a CPU can handle in parallel per clock      
**ILP** (Instruction-level parallelism) is a measure of how many of the instructions in a computer program can be executed simultaneously.      
**Cache line** is the unit of data transfer between the cache and main memory. Typically the cache line is 64 bytes. The processor will read or write an entire cache line when any location in the 64 byte region is read or written. 

![CPU pipelines](https://github.com/khdevnet/dotnet/blob/master/performance/cpu/docs/pipeline.jpg)

#### Resources
* [Making .NET Applications Even Faster](https://www.pluralsight.com/courses/making-dotnet-applications-even-faster)

# Memory data aligment
#### Theory
In programming language, a data object (variable) has 2 properties; its value and the storage location (address). Data alignment means that the address of a data can be evenly divisible by 1, 2, 4, or 8. In other words, data object can have 1-byte, 2-byte, 4-byte, 8-byte alignment or any power of 2.
If the data is misaligned of 4-byte boundary, CPU has to perform extra work to access the data: load 2 chucks of data, shift out unwanted bytes then combine them together. This process definitely slows down the performance and wastes CPU cycle just to get right data from memory.

![Memory aligment](https://github.com/khdevnet/dotnet/blob/master/performance/cpu/docs/memory-align.jpg)

However, the story is a little different for member data in struct, union or class objects. The struct (or union, class) member variables must be aligned to the highest bytes of the size of any member variables to prevent performance penalties. For example, if you have 1 char variable (1-byte) and 1 int variable (4-byte) in a struct, the compiler will pads 3 bytes between these two variables. Therefore, the total size of this struct variable is 8 bytes, instead of 5 bytes. By doing this, the address of this struct data is divisible evenly by 4. This is called structure member alignment. Of course, the size of struct will be grown as a consequence.
```
// size = 4 bytes, alignment = 2-byte, address can be divisible by 2
struct S2 {
    char m1;    // 1-byte
                // padding 1-byte space here
    short m2;   // 2-byte
};

// size = 8 bytes, alignment = 4-byte, address can be divisible by 4
struct S3 {
    char m1;    // 1-byte
                // padding 3-byte space here
    int m2;     // 4-byte
};

```

#### Resources
* [Data Alignment](http://www.songho.ca/misc/alignment/dataalign.html)
