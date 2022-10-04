#pragma once

#include <fstream>
#include <iostream>
#include <string>
#include<string>

using namespace std;

class SimplePuzzle
{
public:
	SimplePuzzle();
	~SimplePuzzle();
	void LoadPuzzle(const string &);
	void PrintPuzzle()const;
	int GetWidth()const;
	int GetHeight()const;
	char GetChar(int, int)const;

private:
	void CreatePuzzle(const string &);
	int gridWidth;
	int gridHeight;
	char **puzzleGrid;
	
};

