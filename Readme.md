# FW Minimal API
Evaluating minimal APIs vs MVC controllers for microservices and updating my own understanding

## Objective

Evaluating Minimal APIs as an alternative and/or complement to the MVC structure.

## Findings

### First Impression
1. Minimal APIs help in reducing the amount of code required in building CRUD operations, making the codebase leaner and easier to maintain.
2. Route definitions are colocated with the handlers, which improves code navigation by keeping everything in one place instead of jumping between files.
3. Less mucking about with the controller/action patterns, resulting in more straightforward implementation.
4. Straightforward implementation overall - less ceremony, more focus on the actual logic.
5. Grouping of routes allows for better searching and testing. Migration from lambda to MapGroup improved code organization and testability.
6. Implementation of DTO pattern prevents over-posting attacks by controlling exactly what data can be sent to the API.
7. Better payload optimization by returning only the data clients need, reducing response sizes.
8. TypedResults provide better return messages and values with compile-time type safety, catching errors earlier in development.


## Implementation Log

### 2026-January-13
1. Completed the tutorial.

### 2026-January-12
1. First commit into master
2. Created the minimal API code
3. Updated the code to MapGroup and calling from Method instead of Lambda
4. Returning using TypedResults