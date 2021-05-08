using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Player
{

    static List<Cell> initCells()
    {
        string[] inputs;
        int numberOfCells = int.Parse(Console.ReadLine()); // 37
        List<Cell> cells = new List<Cell>();
        for (int i = 0; i < numberOfCells; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            cells.Add(new Cell(inputs));
        }
        return cells;
    }

    static List<Tree> initTrees(Day currentDay) {
        List<Tree> trees = new List<Tree>();
        for (int i = 0; i < currentDay.numberOfTrees; i++)
        {
            trees.Add(new Tree());
        }
        return trees;
    }

    static void Main(string[] args)
    {
        List<Cell> cells = initCells();
        foreach (Cell cell in cells) {
            Console.Error.WriteLine(cell.ToString());        }      
        // game loop
        while (true)
        {
            Day currentDay = new Day();
            Console.Error.WriteLine("## CURRENT DAY ##\n" + currentDay.ToString() + "\n" );
            List<Tree> trees = initTrees(currentDay);
            
            Console.Error.WriteLine("## CURRENT TREES ##\n");
            foreach (Tree tree in trees) {
                Console.Error.WriteLine(tree.ToString());
            }
            int numberOfPossibleActions = int.Parse(Console.ReadLine()); // all legal actions
            for (int i = 0; i < numberOfPossibleActions; i++)
            {
                string possibleAction = Console.ReadLine(); // try printing something from here to start with
            }

            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");


            // GROW cellIdx | SEED sourceIdx targetIdx | COMPLETE cellIdx | WAIT <message>
            Console.WriteLine("WAIT");
        }
    }
}