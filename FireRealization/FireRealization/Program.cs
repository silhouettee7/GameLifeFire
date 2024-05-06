using System.Diagnostics;
using Contract;
using FireRealization;

int sizeX = 6;
int sizeY = 6;
var field = new FireField(sizeX, sizeY);
field.Initialize();
int.TryParse(Console.ReadLine(), out int startX);
int.TryParse(Console.ReadLine(), out int startY);

field.SetStartBurningCell(startX, startY);

Cell[,] newField;

while (!field.CheckEndFire())
{
    newField = field.UpdateFieldAfterFire();
    for (int i = 0; i < sizeX; i++)
    {
        for (int j = 0; j < sizeY; j++)
        {
            Console.Write($"{(int)newField[i, j].State}  ");
        }

        Console.WriteLine();
    }

    Console.WriteLine();
    Thread.Sleep(1000);
}


