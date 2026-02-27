# DataStoreBenchmark

DataStoreBenchmark is a .NET 10 application that helps you measure how fast different ways of saving data to a SQL database really are.
It's a simple tool to show you which method works best when you need to insert data.

## What Does It Do?

- Compares several ways to add data to your database:
  - EF Core: Saving each record one at a time
  - EF Core: Adding records one by one, then saving them all together
  - EF Core: Adding a whole batch of records and saving them at once
  - Dapper: Batch insert using Dapper
  - SQL Bulk Copy for super-fast inserts
- Lets you choose how many records to test with
- Shows you exactly how long each method takes

## Benchmark Results

Here's what we found when running the benchmarks:

| Number of Movies | EF Core Add & Save 1 by 1 | EF Core Add 1 by 1, Save Once | EF Core Add Range, Save Once | Dapper     | SQL Bulk Copy |
| ---------------- | ------------------------- | ----------------------------- | ---------------------------- | ---------- | ------------- |
| 100              | 355 ms                    | 18 ms                         | 12 ms                        | 140 ms     | 30 ms         |
| 1,000            | 2,690 ms                  | 89 ms                         | 109 ms                       | 387 ms     | 11 ms         |
| 10,000           | Skipped                   | 1,609 ms                      | 1,584 ms                     | 4,129 ms   | 115 ms        |
| 100,000          | Skipped                   | 9,251 ms                      | 9,125 ms                     | 38,768 ms  | 576 ms        |
| 1,000,000        | Skipped                   | 97,613 ms                     | 97,679 ms                    | 460,239 ms | 5,966 ms      |

**What does this mean?**

- **SQL Bulk Copy** is by far the fastest, especially for big batches.
- **EF Core** performs better than Dapper for batch inserts.
- **Dapper** is slower than EF Core and SQL Bulk Copy.
- Saving each record one at a time (EF Core) is very slow and impractical for large numbers.
