#pragma once

#include <fstream>
#include <iostream>
#include <string>
#include <vector>

using namespace std;

class AdvancedPuzzle
{
public:
	AdvancedPuzzle();
	~AdvancedPuzzle();
	void LoadPuzzle(const string &);
	bool CheckDirections(string &, int, int);
	struct Cell
	{
		char letter;
		int cellX, cellY;
		vector<string> directionString;

	};
	Cell* GetCell(int, int);
	int GetHeight()const;
	int GetWidth()const;

private:
	void CreatePuzzle(string &);
	string getDirectionStrings(string &, int, int, int)const;
	int gridWidth;
	int gridHeight;
	
	//vector<string> puzzleLine;
	vector<Cell*> puzzleCells;

	string puzzleText;
};

