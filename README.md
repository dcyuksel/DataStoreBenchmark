# DataStoreBenchmark

DataStoreBenchmark is a .NET 10 application that helps you measure how fast different ways of saving lots of data to a SQL database really are. It’s a simple tool to show you which method works best when you need to insert a large number of records.

## What Does It Do?

- Compares several ways to add data to your database:
  - Saving each record one at a time
  - Adding records one by one, then saving them all together
  - Adding a whole batch of records and saving them at once
  - Using SQL Bulk Copy for super-fast inserts
- Lets you choose how many records to test with
- Shows you exactly how long each method takes

## Benchmark Results

Here’s what we found when running the benchmarks:

| Number of Movies | Save One by One | Add One by One, Save Once | Add Range, Save Once | SQL Bulk Copy |
|------------------|-----------------|---------------------------|---------------------|--------------|
| 100              | 320 ms          | 17 ms                     | 15 ms               | 25 ms        |
| 1,000            | 3,023 ms        | 108 ms                    | 110 ms              | 10 ms        |
| 10,000           | 51,468 ms       | 847 ms                    | 877 ms              | 112 ms       |
| 100,000          | Skipped         | 9,371 ms                  | 9,327 ms            | 499 ms       |

**What does this mean?**
- **SQL Bulk Copy** is by far the fastest, especially for big batches.
- Saving each record one at a time is very slow for large numbers.
- Saving all at once (after adding one by one or as a range) is much better than saving after every insert.