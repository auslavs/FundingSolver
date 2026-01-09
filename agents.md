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
- **Web Workers** - Offloads brute-force calculation to separate thread for non-blocking UI
- **Brute-force search** - Tests all combinations (0-500 per item) asynchronously

## Distribution

Published to npm as `@auslavs/funding-solver` and distributed via CDN. Can be embedded in any web application as `<funding-solver />`.

## Development Setup

### Prerequisites

1. **Node.js** - Required for npm package management and build tools
2. **.NET 6.0 SDK** - Required for F# compilation via Fable

### Installing .NET SDK

If dotnet is not available in your environment, install it using the official script:

```bash
# Download and run the .NET install script
wget https://dot.net/v1/dotnet-install.sh -O /tmp/dotnet-install.sh
chmod +x /tmp/dotnet-install.sh
/tmp/dotnet-install.sh --channel 6.0 --install-dir $HOME/.dotnet
```

### Setting up PATH

Add dotnet to your PATH for the current session:

```bash
export PATH=$PATH:$HOME/.dotnet:$HOME/.dotnet/tools
```

Or get the command from npm:

```bash
npm run setup-dotnet
```

To make this permanent, add the export to your shell config file (`~/.bashrc` or `~/.zshrc`):

```bash
echo 'export PATH=$PATH:$HOME/.dotnet:$HOME/.dotnet/tools' >> ~/.bashrc
source ~/.bashrc
```

### Development Commands

```bash
# Install dependencies and restore dotnet tools
npm install

# Start development server with hot reload
npm start

# Build for production
npm run build

# Get dotnet PATH setup command
npm run setup-dotnet
```

### Project Structure

- `src/FundingSolver.fs` - Main component logic with Elmish state management
- `src/solver-worker.js` - Web Worker for brute-force calculation
- `src/Tailwind.fs` - Embedded Tailwind CSS
- `src/PreactCustomElement.fs` - Web component registration

## Use Case

Designed for scenarios where specific budget codes or line items must be used to reach a target allocation, such as government funding, grants, or departmental budgets.
