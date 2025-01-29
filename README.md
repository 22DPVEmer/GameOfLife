# Conway's Game of Life

A C# implementation of Conway's Game of Life, developed iteratively.

## Project Overview

This project implements Conway's Game of Life as a console application, following SOLID principles and clean code practices.

## Development Approach

The project is developed in iterations, with each iteration adding new features and improvements.

## Game Rules

1. Any live cell with fewer than two live neighbors dies (underpopulation)
2. Any live cell with two or three live neighbors lives on to the next generation
3. Any live cell with more than three live neighbors dies (overpopulation)
4. Any dead cell with exactly three live neighbors becomes a live cell (reproduction)

## Getting Started

See individual iteration branches for specific implementation details and running instructions.

## Requirements Implemented

1. Game algorithm as defined in [Conway's Game of Life](https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life)
2. Console application with visual grid representation
3. Simple navigation menu for selecting grid size
4. Game field updates every second

## How to Run

1. Clone the repository
2. Open in Visual Studio or your preferred .NET IDE
3. Build and run the project
4. Enter grid dimensions (5-30 for both rows and columns)
5. Watch the patterns evolve!
6. Press 'Q' to quit

## Implementation Details

- Written in C# using .NET
- Follows SOLID principles and clean code practices
- Uses Unicode characters for grid display
- Implements proper error handling and input validation
