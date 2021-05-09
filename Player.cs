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

    public static Dictionary<int,Cell> cellsByIndex  = new Dictionary<int,Cell>();
    public static Dictionary<int,List<Tree>> myTreesBySize = new Dictionary<int,List<Tree>>();
    public static Dictionary<int,Tree> myTreesByindex = new Dictionary<int,Tree>();
    public static Day currentDay;
    public static Dictionary<string, List<Coordinate>> actionByType = new Dictionary<string, List<Coordinate>>();
    public static Dictionary<int, int> costBySize = new Dictionary<int, int>();
    public static Dictionary<int, int> countBySize = new Dictionary<int, int>();

    public static void InitCells()
    {
        int numberOfCells = int.Parse(Console.ReadLine()); // 37
        cellsByIndex.Clear();
        for (int i = 0; i < numberOfCells; i++)
        {
            Cell cell = new Cell(); 
            cellsByIndex[cell.index] = cell; 
        }
        foreach (int key in cellsByIndex.Keys) 
        {
            Console.Error.WriteLine(cellsByIndex[key].ToString());
        }
    }

    private static void InitMyTrees() 
    {
        myTreesBySize.Clear();
        myTreesByindex.Clear();
        Console.Error.WriteLine("## CURRENT TREES ##");
        for (int i = 0; i < currentDay.numberOfTrees; i++)
        {
            Tree tree = new Tree();
            if (tree.isMine) {
                if (!myTreesBySize.ContainsKey(tree.size))
                {
                    myTreesBySize[tree.size] = new List<Tree>();
                }
                myTreesBySize[tree.size].Add(tree);
                myTreesByindex[tree.cellIndex] = tree;
            }
        }
        Dictionary<int,List<Tree>> treesBySizeSorted = new Dictionary<int,List<Tree>>();
        foreach ( int key in myTreesBySize.Keys ) {
            treesBySizeSorted[key] = myTreesBySize[key].OrderBy(tree => tree.cellIndex).ToList();
        }
        foreach ( int key in myTreesByindex.Keys) {
            Console.Error.WriteLine(myTreesByindex[key].ToString());
        }
        Console.Error.WriteLine();
        myTreesBySize = treesBySizeSorted;
    }

    private static int CountTreeBySize(int size) 
    {
        if (myTreesBySize.ContainsKey(size) )
        {
            return myTreesBySize[size].Count;
        }
        return 0; 
    }

    private static void ComputeCountAndCosts() 
    {   
        for ( int i = 0 ; i < 4 ; i++ )
        {
            int count = CountTreeBySize(i);
            countBySize[i] = count;
            int baseCost = 0;
            switch (i)
            {
                case 1 : 
                    baseCost = 1;
                    break;
                case 2 : 
                    baseCost = 3;
                    break;
                case 3 : 
                    baseCost = 7;
                    break;
            }
            costBySize[i] = baseCost + count;
        }
        Console.Error.WriteLine("## COST AND COUNT ##");
        for (int i = 0 ; i < 4 ; i++)
        {
            Console.Error.WriteLine("Tree size 0 : Current count " + countBySize[i] + " | Grow cost " + costBySize[i]);
        }

    }

    private static void InitPossibleActionByType() {
        actionByType.Clear();
        int numberOfPossibleActions = int.Parse(Console.ReadLine()); // all legal actions
        Console.Error.WriteLine("## POSSIBLE ACTIONS ##");
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
            foreach (Coordinate coordinate in sortedActionByType[keyAction]) 
            {
                Console.Error.WriteLine(keyAction + " " + coordinate.ToString());
            }
        }
        actionByType = sortedActionByType;
        Console.Error.WriteLine("\n");
    } 

    private static string ComputeCommand() {
        if (myTreesBySize.ContainsKey(3) && currentDay.sun > 4 && (myTreesBySize[3].Count > 4 || currentDay.index > 22 || (currentDay.index > 20 && myTreesBySize[3].Count > 2))) 
        {
            return "COMPLETE " + myTreesBySize[3][0].cellIndex;
        }
        for (int i = 2 ; i >=0 ; i--)
        {
            if ( myTreesBySize.ContainsKey(i) && myTreesBySize[i].Count > 0 && currentDay.sun > costBySize[i + 1] ) 
            {
                foreach (Tree tree in myTreesBySize[i])
                {
                    if (!tree.isDormant)
                    {
                        return "GROW " + tree.cellIndex;
                    }
                }
            }
        }
        if (currentDay.index > 0 && currentDay.index < 20 && countBySize[0] == 0 && currentDay.sun > costBySize[0] )
        { 
            if (actionByType.ContainsKey("SEED")) 
            {
                List<Coordinate> seedFrom2Or3 = FilterPossibleSeedFromTree1(actionByType["SEED"]);
                foreach(Coordinate coordinate in seedFrom2Or3) 
                {
                    Console.Error.WriteLine("Possible SEED " + coordinate.ToString());
                }
                if(seedFrom2Or3.Count > 0)
                {
                    string bestSeedSoFar = "SEED " + seedFrom2Or3[0].ToString();
                    foreach(Coordinate coordinate in seedFrom2Or3) 
                    {
                        if (!IsAdjacentToMyTree(coordinate.targetIndex))
                        {
                            bestSeedSoFar = "SEED " + coordinate.ToString();
                            break;
                        }
                    }
                    return bestSeedSoFar;
                }
            }
        }            
        return "WAIT ZZzz...";
    }

    private static List<Coordinate> FilterPossibleSeedFromTree1(List<Coordinate> coordinates){
        List<Coordinate> filtered = new List<Coordinate>();
        foreach (Coordinate coordinate in coordinates)
        {
            if (myTreesByindex[coordinate.cellIndex].size > 1 ) 
            {
                filtered.Add(coordinate);
            }
        }
        return filtered;
    }

    private static bool IsAdjacentToMyTree(int targetCellIndex) {
        foreach (int neighboor in cellsByIndex[targetCellIndex].neighboors)
        {
            if (myTreesByindex.ContainsKey(neighboor) && myTreesByindex[neighboor] != null) 
            {
                return true;
            }   
        }        
        return false;
    }


    static void Main(string[] args)
    {
        InitCells();
        // game loop
        while (true)
        {
            currentDay = new Day();
            Console.Error.WriteLine("## CURRENT DAY ##\n" + currentDay.ToString());
            InitMyTrees();
            InitPossibleActionByType();
            ComputeCountAndCosts();
            Console.WriteLine(ComputeCommand());
        }
    }
}