# Redis cache benchmark
* [cache-best-practices](https://docs.microsoft.com/en-us/azure/azure-cache-for-redis/cache-best-practices)
* [redis-benchmark help](https://redis.io/topics/benchmarks)

#### notes
benchmark works only without ssl
```
redis-benchmark -h HostName -p 6379 -d 16000 -a AuthKEY -c 1 -n 1000 -t get
```
