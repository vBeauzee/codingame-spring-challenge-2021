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

    public static List<Cell> cells;
    public static List<Tree> trees;
    public static Day currentDay;

    public static int seedCost = 0;
    public static int growCostTo1 = 0;
    public static int growCostTo2 = 0;
    public static int growCostTo3 = 0;

    public static int numberOfSeeds = 0;
    public static int numberOfTree1 = 0;
    public static int numberOfTree2 = 0;
    public static int numberOfTree3 = 0;

    public static List<Cell> InitCells()
    {
        string[] inputs;
        int numberOfCells = int.Parse(Console.ReadLine()); // 37
        List<Cell> cells = new List<Cell>();
        for (int i = 0; i < numberOfCells; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            cells.Add(new Cell(inputs));
        }
        /*foreach (Cell cell in cells) 
        {
            Console.Error.WriteLine(cell.ToString());
        }*/  
        return cells;
    }

    private static List<Tree> InitTrees(Day currentDay) {
        //Console.Error.WriteLine("## CURRENT TREES ##");
        List<Tree> trees = new List<Tree>();
        for (int i = 0; i < currentDay.numberOfTrees; i++)
        {
            Tree tree = new Tree();
            //Console.Error.WriteLine(tree.ToString());
            trees.Add(tree);
        }
        return trees;
    }

    private static void ComputeCountAndCosts() {
        seedCost = 0;
        growCostTo1 = 1;
        growCostTo2 = 3;
        growCostTo3 = 7;
        numberOfSeeds = 0;
        numberOfTree1 = 0;
        numberOfTree2 = 0;
        numberOfTree3 = 0;
        foreach (Tree tree in trees){
            if (tree.isMine){
                switch (tree.size) 
                {
                    case 0 : 
                        seedCost++;
                        numberOfSeeds++;
                        break;
                    case 1 :
                        growCostTo1++;
                        numberOfTree1++;
                        break;
                    case 2 :
                        growCostTo2++;
                        numberOfTree2++;
                        break;
                    case 3 :
                        growCostTo3++;
                        numberOfTree3++;
                        break;
                }
            }
        }
         Console.Error.WriteLine("Seed : " + numberOfSeeds + " cost : " + seedCost + "\n" + "Tree1 : " + numberOfTree1 + " cost : " + growCostTo1 + "\n" + "Tree2 : " + numberOfTree2 + " cost : " + growCostTo2 + "\n" + "Tree 3 : " + numberOfTree3 + " cost : " + growCostTo3);
    }

    private static Dictionary<string, List<Coordinate>> InitPossibleActionByType() {
        Dictionary<string, List<Coordinate>> actionByType = new Dictionary<string, List<Coordinate>>();
        int numberOfPossibleActions = int.Parse(Console.ReadLine()); // all legal actions
        //Console.Error.WriteLine("## POSSIBLE ACTIONS ##");
        for (int i = 0; i < numberOfPossibleActions; i++)
        {
            string possibleAction = Console.ReadLine(); // try printing something from here to start with
            if ("WAIT" != possibleAction) 
            {
                int indexSeparator = possibleAction.IndexOf(" ");
                string action = possibleAction.Substring(0, indexSeparator);
                if (!actionByType.ContainsKey(action)) 
                {
                    actionByType[action] = new List<Coordinate>();
                }
                string indexAction = possibleAction.Substring(indexSeparator + 1);
                Coordinate coordinate;
                if(indexAction.Contains(" "))
                {
                    string[] indexes = indexAction.Split(" ");
                    coordinate = new Coordinate(indexes[0], indexes[1]);
                } else {
                    coordinate = new Coordinate(indexAction, "-1");
                }
                actionByType[action].Add(coordinate);
            }
        }
        // Sort action by priority
        Dictionary<string, List<Coordinate>> sortedActionByType = new Dictionary<string, List<Coordinate>>();
        foreach (string keyAction in actionByType.Keys) 
        {
            if (keyAction == "SEED") {
                sortedActionByType[keyAction] = actionByType[keyAction].OrderBy(coord => coord.targetIndex).ToList();
            } else {
                sortedActionByType[keyAction] = actionByType[keyAction].OrderBy(coord => coord.cellIndex).ToList();
            }
            /*
            foreach (Coordinate coordinate in sortedActionByType[keyAction]) 
            {
                Console.Error.WriteLine(keyAction + " " + coordinate.ToString());
            }*/
        }
        return sortedActionByType;
    } 
    
    private static bool IsCostPayable(int currentIndex)
    {
        foreach (Tree tree in trees)
        {
            if (tree.cellIndex == currentIndex)
            {
                switch (tree.size)
                {
                    case 3 :
                        return currentDay.sun >= growCostTo3;
                    case 2 :
                        return currentDay.sun >= growCostTo2;
                    case 1 :
                        return currentDay.sun >= growCostTo1;
                    case 0 :
                        return currentDay.sun >= seedCost;
                    default :
                        return false;
                }
            }
        }
        return false;
    }

    static void Main(string[] args)
    {
        cells = InitCells();
        // game loop
        while (true)
        {
            currentDay = new Day();
            //Console.Error.WriteLine("## CURRENT DAY ##\n" + currentDay.ToString());
            trees = InitTrees(currentDay);
            //Console.Error.WriteLine("## COST AS START OF DAY ##" + "\n" + "SeedCost : " + seedCost+  " | 0 to 1 : " + growCostTo1 + " | 1 to 2 : " + growCostTo2 + " | 2 to 3 : " + growCostTo3 );
            Dictionary<string, List<Coordinate>> actionByType = InitPossibleActionByType();
            ComputeCountAndCosts();
            string currentCommand = "WAIT";
            if ((currentDay.index > 21 || numberOfTree3 > 4 ) && actionByType.ContainsKey("COMPLETE")) {
                foreach (Coordinate coordinate in actionByType["COMPLETE"]) {
                    if (coordinate.cellIndex < 7 && currentDay.sun > 3) 
                    {
                        currentCommand = "COMPLETE " + coordinate.cellIndex;
                        break;
                    } else if (coordinate.cellIndex < 19 && currentDay.sun > 3) 
                    {
                        currentCommand = "COMPLETE " + coordinate.cellIndex;
                        break;
                    } else if ( currentDay.sun > 3 ) {
                        currentCommand = "COMPLETE " + coordinate.cellIndex;
                        break;
                    }
                } 
            }
            if (currentCommand == "WAIT" && actionByType.ContainsKey("GROW")) 
            {
                foreach (Coordinate coordinate in actionByType["GROW"]) 
                {
                    if (coordinate.cellIndex < 7 && IsCostPayable(coordinate.cellIndex)) 
                    {
                        currentCommand = "GROW " + coordinate.cellIndex;
                        break;
                    } else if (coordinate.cellIndex < 19 && IsCostPayable(coordinate.cellIndex)) 
                    {
                        currentCommand = "GROW " + coordinate.cellIndex;
                        break;
                    } else if ( IsCostPayable(coordinate.cellIndex)) 
                    {
                        currentCommand = "GROW " + coordinate.cellIndex;
                        break;
                    }
                }
            } 
            if ( numberOfSeeds < 2 && currentDay.index < 19 && currentCommand == "WAIT" && actionByType.ContainsKey("SEED")) {
                foreach (Coordinate coordinate in actionByType["SEED"]) 
                {
                    if (coordinate.targetIndex < 7  &&  currentDay.sun >= seedCost) 
                    {
                        currentCommand = "SEED " + coordinate.cellIndex + " " + coordinate.targetIndex;
                        seedCost++;
                        break;
                    }  else if (coordinate.targetIndex < 19  && currentDay.sun >= seedCost) 
                    {
                        currentCommand = "SEED " + coordinate.cellIndex + " " + coordinate.targetIndex;
                        seedCost++;
                        break;
                    } else if (currentDay.sun >= seedCost ) {
                        currentCommand = "SEED " + coordinate.cellIndex + " " + coordinate.targetIndex;
                        seedCost++;
                        break;
                    }                   
                }
            }

            // GROW cellIdx | SEED sourceIdx targetIdx | COMPLETE cellIdx | WAIT <message>
            Console.WriteLine(currentCommand);
        }
    }
}
