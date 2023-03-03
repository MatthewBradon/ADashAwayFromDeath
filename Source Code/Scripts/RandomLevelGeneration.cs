using Godot;
using System;
using System.Collections.Generic;
using Scripts.LevelGenerationFunctions;

public class RandomLevelGeneration : Node2D
{
    private int WIDTH = 5;
    private int HEIGHT = 5;

    public const int ROOM_WIDTH = 512;
    public const int ROOM_HEIGHT = 352;

    public List<List<int>> map;
    public int startPosition = 0;

    public enum EVENTS { DEFAULT, PITS};
    public enum ROOMS {START, FILL, LEFTRIGHT, LEFTRIGHTBOTTOM, LEFTRIGHTTOP, LEFTRIGHTBOTTOMTOP, PITTOP, PITCENTER, PITBOTTOM, END};
    private RandomMethods rand;
    Node2D rooms;

    LevelCount levelCount;
    [Signal] public delegate void mapDrawn();
    
    public override void _Ready()
    {
        rooms = GetNode<Node2D>("rooms");
        rand = new RandomMethods();
        rand.createRandom();

        levelCount = (LevelCount)GetNode("/root/LevelCountSingleton");

        

        
        generateCriticalPath();
        placeExtraRooms();
        generateBorder();
        printMap();


    }
    public void generateCriticalPath(){
        int currentX = 0;
        int currentY = 0;
        int nextRoom = 0; //1 = left, 2 = right, 3 = bottom
        bool alreadyMoved = false;
        bool finished = false;

        map = new List<List<int>>();

        HEIGHT = levelCount.getCurrentHeight() + (levelCount.getLevelCount() % 5 == 0 ? 1 : 0);
        levelCount.setCurrentHeight(HEIGHT);


        //fill map with fill
        for(int i = 0; i < HEIGHT; i++){
            map.Add(new List<int>());
            for(int j = 0; j < WIDTH; j++){
                map[i].Add((int)ROOMS.FILL);
            }
        }

       

        startPosition = rand.random(0, WIDTH-2);
        map[0][startPosition] = (int)ROOMS.START;
        currentX = startPosition;



        while(!finished){
            //First move
            if(!alreadyMoved){
                //Check if we are in the Corner
                if(currentX > 0 && currentX < WIDTH - 1){
                    nextRoom = rand.random(1, 3);
                    if(nextRoom == 1){
                        currentX--;
                    } else {
                        currentX++;
                    }

                }
                else if(currentX == 0){
                    currentX++;
                }
                else if(currentY == WIDTH - 1){
                    currentX--;
                }
                map[currentY][currentX] = (int)ROOMS.LEFTRIGHT;
                alreadyMoved = true;
            }
            //Next steps
            else if(currentY < HEIGHT - 1){
                if(currentX == 0 || currentX < WIDTH - 1){
                    if((currentY > 0 && map[currentY-1][currentX] == (int)ROOMS.LEFTRIGHTBOTTOM) || (currentY > 0 && map[currentY - 1][currentX] == (int)ROOMS.LEFTRIGHTBOTTOMTOP)){
                        map[currentY][currentX] =  (int)ROOMS.LEFTRIGHTBOTTOMTOP;
                    } else {
                        map[currentY][currentX] =  (int)ROOMS.LEFTRIGHTBOTTOM;
                    }

                    currentY++;
                    map[currentY][currentX] = (int)ROOMS.LEFTRIGHTTOP;
                    alreadyMoved = false;


                }
                else {
                    int way = rand.random(1, 2);
                    if(way == 1 && currentX > 0 && currentX < WIDTH - 1){ 
                        if(map[currentY][currentX-1] == (int)ROOMS.FILL){
                            currentX--;
                            map[currentY][currentX] = (int)ROOMS.LEFTRIGHT;
                        }
                        else {;
                            currentX++;
                            map[currentY][currentX] = (int)ROOMS.LEFTRIGHT;

                        }
                    }
                    else {
                        if((currentY > 0 && map[currentY - 1][currentX] == (int)ROOMS.LEFTRIGHTBOTTOM) || (currentY > 0 && map[currentY - 1][currentX] == (int)ROOMS.LEFTRIGHTBOTTOMTOP)){
                            map[currentY][currentX] = (int)ROOMS.LEFTRIGHTBOTTOMTOP;
                        } else {
                            map[currentY][currentX] = (int)ROOMS.LEFTRIGHTBOTTOM;
                        }

                        currentY++;
                        map[currentY][currentX] = (int)ROOMS.LEFTRIGHTTOP;
                        alreadyMoved = false;
                    }
                }
            }
            else {
                if(currentX > 0  && currentY < WIDTH - 1){
                    nextRoom = rand.random(1, 3);
                    if(nextRoom == 1){
                        currentX--;
                    } else {
                        currentX++;
                    }
                    map[currentY][currentX] = (int)ROOMS.LEFTRIGHT;
                }
                else {
                    map[currentY][currentX] = (int)ROOMS.END;
                    finished = true;
                }
            }
        }
        /*
        foreach(List<int> row in map){
            foreach(int room in row){
                GD.Print(room);
            }
            GD.Print(" ");
        }
        */

    }
    private void placeMap(String pathName, int offsetX, int offsetY){
        PackedScene room = (PackedScene)ResourceLoader.Load(pathName);
        Node2D roomInstance = (Node2D)room.Instance();
        rooms.AddChild(roomInstance);
        roomInstance.Position = new Vector2(offsetX, offsetY);
    }
    private void placeExtraRooms(){
        for(int i = 0; i < HEIGHT; i++){
            for(int j = 0; j < WIDTH; j++){
                if(map[i][j] == (int)ROOMS.FILL){
                    switch(rand.random(1, 4)){
                        case 1:
                            map[i][j] = (int)ROOMS.LEFTRIGHT;
                            break;
                        case 2:
                            if(i < 4){
                                if(map[i+1][j] == (int)ROOMS.LEFTRIGHTBOTTOM){
                                    map[i][j] = (int)ROOMS.LEFTRIGHTBOTTOM;
                                    map[i+1][j] = (int)ROOMS.LEFTRIGHTBOTTOMTOP;
                                } 
                            }
                             else {
                                    map[i][j] = (int)ROOMS.LEFTRIGHTBOTTOM;
                                }
                            break;
                        case 3:
                            if(i > 0){
                                if(map[i-1][j] == (int)ROOMS.LEFTRIGHTTOP){
                                    map[i][j] = (int)ROOMS.LEFTRIGHTTOP;
                                    map[i-1][j] = (int)ROOMS.LEFTRIGHTBOTTOMTOP;
                                }
                            }
                            else {
                                map[i][j] = (int)ROOMS.LEFTRIGHTTOP;
                            }
                            break;
                        case 4:
                            map[i][j] = (int)ROOMS.FILL;
                            break;
                    }
                }
            }
        }
    }
    public void printMap(){
        String pathName = "";
        //generationTimeoutTimer(drawDelay);
        for(int i = 0; i < HEIGHT+2; i++){
            for(int j = 0; j < WIDTH+2; j++){
                switch(map[i][j]){
                    case (int)ROOMS.START:
                        int startRoom = rand.random(1,1);
                        pathName = "res://Scenes/rooms/start/start" + startRoom + ".tscn";
                        
                        break;
                    case (int)ROOMS.FILL:
                        int fillRoom = rand.random(1,1);
                        pathName = "res://Scenes/rooms/fill/fill" + fillRoom + ".tscn";
                        break;
                    case (int)ROOMS.LEFTRIGHT:
                        int leftRightRoom = rand.random(1, 5);
                        pathName = "res://Scenes/rooms/left_right/left_right" + leftRightRoom + ".tscn";
                        break;
                    case (int)ROOMS.LEFTRIGHTBOTTOM:
                        int leftRightBottomRoom = rand.random(1,5);
                        pathName = "res://Scenes/rooms/left_right_bottom/left_right_bottom" + leftRightBottomRoom + ".tscn";
                        break;
                    case (int)ROOMS.LEFTRIGHTTOP:
                        int leftRightTopRoom = rand.random(1, 5);
                        pathName = "res://Scenes/rooms/left_right_top/left_right_top" + leftRightTopRoom + ".tscn";
                        break;
                    case (int)ROOMS.LEFTRIGHTBOTTOMTOP:
                        int leftRightBottomTopRoom = rand.random(1, 1);
                        pathName = "res://Scenes/rooms/left_right_top_bottom/left_right_top_bottom" + leftRightBottomTopRoom + ".tscn";
                        break;
                    case (int)ROOMS.END:
                        int endRoom = rand.random(1, 2);
                        pathName = "res://Scenes/rooms/end/end" + endRoom + ".tscn";
                        break;
                }
                placeMap(pathName, j * ROOM_WIDTH, i * ROOM_HEIGHT);
    
            }
        }
        EmitSignal("mapDrawn");
    }
    private void generateBorder(){
        List<List<int>> newMap = new List<List<int>>();
        //fill map with fill
        for(int i = 0; i < HEIGHT+2; i++){
            newMap.Add(new List<int>());
            for(int j = 0; j < WIDTH+2; j++){
                newMap[i].Add((int)ROOMS.FILL);
            }
        }

        //fill newMap with old map with the apporiate offset to make room for the border
        for(int i = 0; i < HEIGHT; i++){
            for(int j = 0; j < WIDTH; j++){
                newMap[i+1][j+1] = map[i][j];
            }
        }
        map = newMap;
    }
    public void setHeight(int height){
        HEIGHT = height;
    }
}