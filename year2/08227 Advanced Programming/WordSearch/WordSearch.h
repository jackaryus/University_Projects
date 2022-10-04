#pragma once

#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <Windows.h>
#include "SimplePuzzle.h"
#include "AdvancedPuzzle.h"
#include "AdvancedDictionary.h"

using namespace std;

class WordSearch
{
public:
	WordSearch();
	~WordSearch();

	bool ReadSimplePuzzle();
	bool ReadSimpleDictionary();
	bool ReadAdvancedPuzzle();
	bool ReadAdvancedDictionary();

	bool SolveSimplePuzzleWithSimpleDictionary();
	bool SolveSimplePuzzleWithAdvancedDictionary();
	bool SolveAdvancedPuzzleWithSimpleDictionary();
	bool SolveAdvancedPuzzleWithAdvancedDictionary();

	void WriteResults(const string &);

private:
	LARGE_INTEGER start, end, frequency;
	vector<string> resultList;
	const int NUMBER_OF_RUNS;
	const string PUZZLE_NAME;
	const string DICTIONARY_NAME;
	SimplePuzzle simplePuzzle;
	AdvancedPuzzle advancedPuzzle;
	vector<string> simpleDictionary;
	AdvancedDictionary advancedDictionary;
	double timeTakenInSeconds;
	double popTimeTakenInSecondsSimple;
	double popTimeTakenInSecondsAdvanced;
	double popTimeTakenInSeconds;
	int noOfVisitedCells;
	int noOfVisitedEntries;
};

