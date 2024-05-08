using System.Diagnostics;
using FireRealization;

int sizeX = 500;
int sizeY = 500;
var field = new FireField(sizeX, sizeY);
field.Initialize();
int.TryParse(Console.ReadLine(), out int startX);
int.TryParse(Console.ReadLine(), out int startY);

field.SetStartBurningCell(startX, startY);


var watch = new Stopwatch();
watch.Start();
field.UpdateFieldAfterFire();
watch.Stop();
Console.WriteLine(watch.ElapsedTicks);
Console.ReadLine();
