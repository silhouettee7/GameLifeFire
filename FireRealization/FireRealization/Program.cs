using System.Diagnostics;
using Contract;
using FireRealization;

int sizeX = 2000;
int sizeY = 2000;
var field = new FireField(sizeX, sizeY);
field.Initialize();
int.TryParse(Console.ReadLine(), out int startX);
int.TryParse(Console.ReadLine(), out int startY);

field.SetStartBurningCell(startX, startY);


var watch = new Stopwatch();
watch.Start();
/*while (!field.CheckEndFire())
{
    field.UpdateFieldWithoutParallel();
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
}*/
field.UpdateFieldAfterFire();
watch.Stop();
Console.WriteLine(watch.ElapsedTicks);

