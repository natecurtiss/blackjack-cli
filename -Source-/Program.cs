using Blackjack;
using static System.Console;

Title = "Blackjack";
new Game(100, Clear, WriteLine, ReadLine!, errorMessage =>
{
    var i = 0;
    while (!int.TryParse(ReadLine(), out i))
        WriteLine(errorMessage);
    return i;
}).Start();