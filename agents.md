# Funding Solver Agents

## Project Intent

Funding Solver is a mathematical solver for budget allocation problems. Given three line items with fixed prices and a target total cost, it finds all valid quantity combinations that exactly match the target.

## Core Problem

Solves: `(Price_A × Qty_A) + (Price_B × Qty_B) + (Price_C × Qty_C) = Target Cost`

Where:
- Line items have fixed prices ($214.41, $193.99, $74.63)
- Quantities range from 0-500
- Target cost is user-defined

## Technical Approach

Built as a reusable web component using:
- **F# + Fable** - Functional programming compiled to JavaScript
- **Elmish** - Predictable state management
- **Preact** - Lightweight UI rendering
- **Brute-force search** - Tests all combinations asynchronously

## Distribution

Published to npm as `@auslavs/funding-solver` and distributed via CDN. Can be embedded in any web application as `<funding-solver />`.

## Use Case

Designed for scenarios where specific budget codes or line items must be used to reach a target allocation, such as government funding, grants, or departmental budgets.
