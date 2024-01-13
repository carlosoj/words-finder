# Words finder

This emulates the game of "Letters soup", ny receiving a matrix of NxN and a list of words, if will search (from left to right, and from top to bottom) if any of the words appear appear. It also counts how many times a word appears and then returns the top 10 words that appear ordered in descending order, from the one that appears the most to the less repeated one.

The solution contains 3 projects:
 - WordsFinder: Contains all the business logic
 - WordsFinder.Tests: All the tests
 - WordsFinder.Console: a console application that you can launch and use to test the project.

 
## Run Locally

Clone the project

```bash
  git clone https://github.com/carlosoj/words-finder.git
```

Go to the project directory

```bash
  cd WordsFinder.Console
```

Install dependencies

```bash
  nuget restore
```

Start the server

```bash
  dotnet run WordsFinder.csproj
```

## Screenshots

![Start scree](https://photos.app.goo.gl/pb6ZA83h5vn7Hw4H6)
