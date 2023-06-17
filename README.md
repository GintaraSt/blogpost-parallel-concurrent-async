# blogpost-parallel-concurrent-async
Samples for blog post comparing parallel concurrent and asynchronous computing.

This repository contains code to test the differences between parallel, concurrent and asynchronous execution when using CPU bound tasks and tasks that are asynchronous by nature.

## Projects
This repository contains two C# projects: `ParallelVsConcurrentVsAsync` and `TestAPI`

`ParallelVsConcurrentVsAsync` - contains code to test different combinations of parallel/concurrent/asynchronous and cpu-bound/asynchronous tasks.

`TestAPI` - contains .NET minimal API which is used when testing asynchronous taks, as we are using simple http requests with delayed response.
