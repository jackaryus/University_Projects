#include "WordSearch.h"
#include <algorithm>
#include <iterator>
#include <iostream>
#include <iomanip>
#include <fstream>

#include<string>

using namespace std;

WordSearch::WordSearch() : start(), end(), frequency(), NUMBER_OF_RUNS(500), PUZZLE_NAME("wordsearch_grid.txt"), DICTIONARY_NAME("dictionary.txt"), timeTakenInSeconds(0), 
popTimeTakenInSecondsSimple(0), popTimeTakenInSecondsAdvanced(0), popTimeTakenInSeconds(0), noOfVisitedCells(0), noOfVisitedEntries(0)
{
}

WordSearch::~WordSearch()
{
}

bool WordSearch::ReadAdvancedPuzzle()
{
	//cout << endl << "ReadAdvancedPuzzle() has NOT been implemented" << endl;
	popTimeTakenInSecondsAdvanced = 0;
	QueryPerformanceFrequency(&frequency);
	QueryPerformanceCounter(&start);

	advancedPuzzle.LoadPuzzle(PUZZLE_NAME);
	cout << endl;

	QueryPerformanceCounter(&end);
	popTimeTakenInSecondsAdvanced = (end.QuadPart - start.QuadPart) / (double)(frequency.QuadPart);
	return true;
}

bool WordSearch::ReadSimplePuzzle()
{
	//cout << endl << "ReadSimplePuzzle() has NOT been implemented" << endl;
	popTimeTakenInSecondsSimple = 0;
	QueryPerformanceFrequency(&frequency);
	QueryPerformanceCounter(&start);

	simplePuzzle.LoadPuzzle(PUZZLE_NAME);
	//simplePuzzle.PrintPuzzle();
	cout << endl;

	QueryPerformanceCounter(&end);
	popTimeTakenInSecondsSimple = (end.QuadPart - start.QuadPart) / (double)(frequency.QuadPart);

	return true;
}

bool WordSearch::ReadAdvancedDictionary()
{
	//cout << endl << "ReadAdvancedDictionary() has NOT been implemented" << endl;
	cout << "Advanced Dictionary" << endl;
	advancedDictionary.LoadDictionary(DICTIONARY_NAME);
	advancedDictionary.PrintAdvancedDictionaryPublic();
	cout << endl;
	return true;
}

bool WordSearch::ReadSimpleDictionary()
{
	//cout << endl << "ReadSimpleDictionary() has NOT been implemented" << endl;
	cout << "Simple Dictionary" << endl;
	ifstream file;
	file.open(DICTIONARY_NAME);
	if (file.is_open())
	{
		string line;
		while (getline(file, line))
		{
			simpleDictionary.push_back(line);
		}
		vector<string>::iterator i;
		for (i = simpleDictionary.begin(); i != simpleDictionary.end(); i++)
		{
			string word = *i;
			cout << word << endl;
		}

	}
	return true;
}

bool WordSearch::SolveSimplePuzzleWithSimpleDictionary() {

	//cout << endl << "SolveSimplePuzzleWithSimpleDictionary() has NOT been implemented" << endl;
	cout << "simplePuzzle + simpleDictionary" << endl;
	timeTakenInSeconds = 0;
	QueryPerformanceFrequency(&frequency);
	QueryPerformanceCounter(&start);

	vector<string>::iterator i;
	string word;
	int currentX;
	int currentY;
	int x;
	int y;
	//vector<string> resultList;
	string result;
	bool matchedWord;

	for (int n = 0; n < NUMBER_OF_RUNS; ++n) {
		// Add your solving code here!
		resultList.clear();
		noOfVisitedCells = 0;
		noOfVisitedEntries = 0;
		for (y = 0; y < simplePuzzle.GetHeight(); y++)
		{
			for (x = 0; x < simplePuzzle.GetWidth(); x++)
			{
				noOfVisitedCells++;
				for (i = simpleDictionary.begin(); i != simpleDictionary.end(); i++)
				{
					noOfVisitedEntries++;
					word = *i;
					if (simplePuzzle.GetChar(x, y) == word[0])
					{
						if (simplePuzzle.GetChar(x, y + 1) == word[1])
						{
							currentX = x;
							currentY = y + 1;
							noOfVisitedCells++;
							matchedWord = true;
							for (int c = 2; c < word.length(); c++)
							{
								currentY += 1;
								noOfVisitedCells++;
								if (simplePuzzle.GetChar(currentX, currentY) != word[c])
								{
									matchedWord = false;
									break;
								}
							}
							if (matchedWord == true)
							{
								result = to_string(x) + " " + to_string(y) + " " + word;
								resultList.push_back(result);
							}
						}
						if (simplePuzzle.GetChar(x + 1, y + 1) == word[1])
						{
							currentX = x + 1;
							currentY = y + 1;
							noOfVisitedCells++;
							matchedWord = true;
							for (int c = 2; c < word.length(); c++)
							{
								currentX += 1;
								currentY += 1;
								noOfVisitedCells++;
								if (simplePuzzle.GetChar(currentX, currentY) != word[c])
								{
									matchedWord = false;
									break;
								}
							}
							if (matchedWord == true)
							{
								result = to_string(x) + " " + to_string(y) + " " + word;
								resultList.push_back(result);
							}
						}
						if (simplePuzzle.GetChar(x + 1, y) == word[1])
						{
							currentX = x + 1;
							currentY = y;
							noOfVisitedCells++;
							matchedWord = true;
							for (int c = 2; c < word.length(); c++)
							{
								currentX += 1;
								noOfVisitedCells++;
								if (simplePuzzle.GetChar(currentX, currentY) != word[c])
								{
									matchedWord = false;
									break;
								}
							}
							if (matchedWord == true)
							{
								result = to_string(x) + " " + to_string(y) + " " + word;
								resultList.push_back(result);
							}
						}
						if (simplePuzzle.GetChar(x + 1, y - 1) == word[1])
						{
							currentX = x + 1;
							currentY = y - 1;
							noOfVisitedCells++;
							matchedWord = true;
							for (int c = 2; c < word.length(); c++)
							{
								currentX += 1;
								currentY -= 1;
								noOfVisitedCells++;
								if (simplePuzzle.GetChar(currentX, currentY) != word[c])
								{
									matchedWord = false;
									break;
								}
							}
							if (matchedWord == true)
							{
								result = to_string(x) + " " + to_string(y) + " " + word;
								resultList.push_back(result);
							}
						}
						if (simplePuzzle.GetChar(x, y - 1) == word[1])
						{
							currentX = x;
							currentY = y - 1;
							noOfVisitedCells++;
							matchedWord = true;
							for (int c = 2; c < word.length(); c++)
							{
								currentY -= 1;
								noOfVisitedCells++;
								if (simplePuzzle.GetChar(currentX, currentY) != word[c])
								{
									matchedWord = false;
									break;
								}
							}
							if (matchedWord == true)
							{
								result = to_string(x) + " " + to_string(y) + " " + word;
								resultList.push_back(result);
							}
						}
						if (simplePuzzle.GetChar(x - 1, y - 1) == word[1])
						{
							currentX = x - 1;
							currentY = y - 1;
							noOfVisitedCells++;
							matchedWord = true;
							for (int c = 2; c < word.length(); c++)
							{
								currentX -= 1;
								currentY -= 1;
								noOfVisitedCells++;
								if (simplePuzzle.GetChar(currentX, currentY) != word[c])
								{
									matchedWord = false;
									break;
								}
							}
							if (matchedWord == true)
							{
								result = to_string(x) + " " + to_string(y) + " " + word;
								resultList.push_back(result);
							}
						}
						if (simplePuzzle.GetChar(x - 1, y) == word[1])
						{
							currentX = x - 1;
							currentY = y;
							noOfVisitedCells++;
							matchedWord = true;
							for (int c = 2; c < word.length(); c++)
							{
								currentX -= 1;
								noOfVisitedCells++;
								if (simplePuzzle.GetChar(currentX, currentY) != word[c])
								{
									matchedWord = false;
									break;
								}
							}
							if (matchedWord == true)
							{
								result = to_string(x) + " " + to_string(y) + " " + word;
								resultList.push_back(result);
							}
						}
						if (simplePuzzle.GetChar(x - 1, y + 1) == word[1])
						{
							currentX = x - 1;
							currentY = y + 1;
							noOfVisitedCells++;
							matchedWord = true;
							for (int c = 2; c < word.length(); c++)
							{
								currentX -= 1;
								currentY += 1;
								noOfVisitedCells++;
								if (simplePuzzle.GetChar(currentX, currentY) != word[c])
								{
									matchedWord = false;
									break;
								}
							}
							if (matchedWord == true)
							{
								result = to_string(x) + " " + to_string(y) + " " + word;
								resultList.push_back(result);
							}
						}
					}
				}
			}
		}
	}
	for (i = resultList.begin(); i != resultList.end(); i++)
	{
		string value = *i;
		cout << value << endl;
	}
	popTimeTakenInSeconds = popTimeTakenInSecondsSimple;

	QueryPerformanceCounter(&end);
	timeTakenInSeconds = (end.QuadPart - start.QuadPart) / (double)(frequency.QuadPart*NUMBER_OF_RUNS);

	////////////////////////////////////////////////
	// This output should be to your results file //
	////////////////////////////////////////////////
	cout << fixed << setprecision(10) << "SolveSimplePuzzleWithSimpleDictionary() - " << timeTakenInSeconds << " seconds" << endl;
	cout << endl;
	return true; // change to true to write to file
}

bool WordSearch::SolveSimplePuzzleWithAdvancedDictionary() 
{
	//cout << endl << "SolveSimplePuzzleWithAdvancedDictionary() has NOT been implemented" << endl;
	cout << "simplePuzzle + advancedDictionary" << endl;
	timeTakenInSeconds = 0;
	QueryPerformanceFrequency(&frequency);
	QueryPerformanceCounter(&start);

	vector<string>::iterator i;
	string word;
	int currentX;
	int currentY;
	int x;
	int y;
	//vector<string> resultList;
	string result;

	for (int n = 0; n < NUMBER_OF_RUNS; ++n) {
		// Add your solving code here!
		resultList.clear();
		noOfVisitedCells = 0;
		noOfVisitedEntries = 0;
		for (y = 0; y < simplePuzzle.GetHeight(); y++)
		{
			for (x = 0; x < simplePuzzle.GetWidth(); x++)
			{
				word = "";
				noOfVisitedCells++;
				AdvancedDictionary::CharNodes *startNode = advancedDictionary.GetNode(NULL, simplePuzzle.GetChar(x, y));
				noOfVisitedEntries++;
				if (startNode != NULL)
				{
					word += startNode->letter;
					AdvancedDictionary::CharNodes *currentNode = advancedDictionary.GetNode(startNode, simplePuzzle.GetChar(x, y + 1));
					noOfVisitedEntries++;
					currentX = x;
					currentY = y + 1;
					noOfVisitedCells++;
					while (currentNode != NULL)
					{
						word += currentNode->letter;
						//cout << word << endl;
						if (currentNode->endLetter)
						{
							result = to_string(x) + " " + to_string(y) + " " + word;
							resultList.push_back(result);
						}
						currentY += 1;
						noOfVisitedCells++;
						currentNode = advancedDictionary.GetNode(currentNode, simplePuzzle.GetChar(currentX, currentY));
						noOfVisitedEntries++;
					}

					word = "";
					word += startNode->letter;
					currentNode = advancedDictionary.GetNode(startNode, simplePuzzle.GetChar(x + 1, y + 1));
					noOfVisitedEntries++;
					currentX = x + 1;
					currentY = y + 1;
					noOfVisitedCells++;
					while (currentNode != NULL)
					{
						word += currentNode->letter;
						if (currentNode->endLetter)
						{
							result = to_string(x) + " " + to_string(y) + " " + word;
							resultList.push_back(result);
						}
						currentX += 1;
						currentY += 1;
						noOfVisitedCells++;
						currentNode = advancedDictionary.GetNode(currentNode, simplePuzzle.GetChar(currentX, currentY));
						noOfVisitedEntries++;
					}

					word = "";
					word += startNode->letter;
					currentNode = advancedDictionary.GetNode(startNode, simplePuzzle.GetChar(x + 1, y));
					noOfVisitedEntries++;
					currentX = x + 1;
					currentY = y;
					noOfVisitedCells++;
					while (currentNode != NULL)
					{
						word += currentNode->letter;
						if (currentNode->endLetter)
						{
							result = to_string(x) + " " + to_string(y) + " " + word;
							resultList.push_back(result);
						}
						currentX += 1;
						noOfVisitedCells++;
						currentNode = advancedDictionary.GetNode(currentNode, simplePuzzle.GetChar(currentX, currentY));
						noOfVisitedEntries++;
					}

					word = "";
					word += startNode->letter;
					currentNode = advancedDictionary.GetNode(startNode, simplePuzzle.GetChar(x + 1, y - 1));
					noOfVisitedEntries++;
					currentX = x + 1;
					currentY = y - 1;
					noOfVisitedCells++;
					while (currentNode != NULL)
					{
						word += currentNode->letter;
						if (currentNode->endLetter)
						{
							result = to_string(x) + " " + to_string(y) + " " + word;
							resultList.push_back(result);
						}
						currentX += 1;
						currentY -= 1;
						noOfVisitedCells++;
						currentNode = advancedDictionary.GetNode(currentNode, simplePuzzle.GetChar(currentX, currentY));
						noOfVisitedEntries++;
					}

					word = "";
					word += startNode->letter;
					currentNode = advancedDictionary.GetNode(startNode, simplePuzzle.GetChar(x, y - 1));
					noOfVisitedEntries++;
					currentX = x;
					currentY = y - 1;
					noOfVisitedCells++;
					while (currentNode != NULL)
					{
						word += currentNode->letter;
						if (currentNode->endLetter)
						{
							result = to_string(x) + " " + to_string(y) + " " + word;
							resultList.push_back(result);
						}
						currentY -= 1;
						noOfVisitedCells++;
						currentNode = advancedDictionary.GetNode(currentNode, simplePuzzle.GetChar(currentX, currentY));
						noOfVisitedEntries++;
					}
					
					word = "";
					word += startNode->letter;
					currentNode = advancedDictionary.GetNode(startNode, simplePuzzle.GetChar(x - 1, y - 1));
					noOfVisitedEntries++;
					currentX = x - 1;
					currentY = y - 1;
					noOfVisitedCells++;
					while (currentNode != NULL)
					{
						word += currentNode->letter;
						if (currentNode->endLetter)
						{
							result = to_string(x) + " " + to_string(y) + " " + word;
							resultList.push_back(result);
						}
						currentX -= 1;
						currentY -= 1;
						noOfVisitedCells++;
						currentNode = advancedDictionary.GetNode(currentNode, simplePuzzle.GetChar(currentX, currentY));
						noOfVisitedEntries++;
					}

					word = "";
					word += startNode->letter;
					currentNode = advancedDictionary.GetNode(startNode, simplePuzzle.GetChar(x - 1, y));
					noOfVisitedEntries++;
					currentX = x - 1;
					currentY = y;
					noOfVisitedCells++;
					while (currentNode != NULL)
					{
						word += currentNode->letter;
						if (currentNode->endLetter)
						{
							result = to_string(x) + " " + to_string(y) + " " + word;
							resultList.push_back(result);
						}
						currentX -= 1;
						noOfVisitedCells++;
						currentNode = advancedDictionary.GetNode(currentNode, simplePuzzle.GetChar(currentX, currentY));
						noOfVisitedEntries++;
					}

					word = "";
					word += startNode->letter;
					currentNode = advancedDictionary.GetNode(startNode, simplePuzzle.GetChar(x - 1, y + 1));
					noOfVisitedEntries++;
					currentX = x - 1;
					currentY = y + 1;
					noOfVisitedCells++;
					while (currentNode != NULL)
					{
						word += currentNode->letter;
						if (currentNode->endLetter)
						{
							result = to_string(x) + " " + to_string(y) + " " + word;
							resultList.push_back(result);
						}
						currentX -= 1;
						currentY += 1;
						noOfVisitedCells++;
						currentNode = advancedDictionary.GetNode(currentNode, simplePuzzle.GetChar(currentX, currentY));
						noOfVisitedEntries++;
					}
				}
			}
		}
	}
	for (i = resultList.begin(); i != resultList.end(); i++)
	{
		string value = *i;
		cout << value << endl;
	}

	popTimeTakenInSeconds = popTimeTakenInSecondsSimple;

	QueryPerformanceCounter(&end);
	timeTakenInSeconds = (end.QuadPart - start.QuadPart) / (double)(frequency.QuadPart*NUMBER_OF_RUNS);

	////////////////////////////////////////////////
	// This output should be to your results file //
	////////////////////////////////////////////////
	cout << fixed << setprecision(10) << "SolveSimplePuzzleWithAdvancedDictionary() - " << timeTakenInSeconds << " seconds" << endl;
	cout << endl;
	return true; // change to true to write to file
}

bool WordSearch::SolveAdvancedPuzzleWithSimpleDictionary() 
{	
	cout << "AdvancedPuzzle + simpleDictionary" << endl;
	string result;
	timeTakenInSeconds = 0;
	QueryPerformanceFrequency(&frequency);
	QueryPerformanceCounter(&start);
	vector<string>::iterator i;
	for (int n = 0; n < NUMBER_OF_RUNS; ++n)
	{
		// Add your solving code here!
		resultList.clear();
		noOfVisitedCells = 0;
		noOfVisitedEntries = 0;
		for (int y = 0; y < advancedPuzzle.GetHeight(); y++)
		{
			for (int x = 0; x < advancedPuzzle.GetWidth(); x++)
			{
				noOfVisitedCells++;
				for (i = simpleDictionary.begin(); i != simpleDictionary.end(); i++)
				{
					noOfVisitedEntries++;
					string word = *i;
					if (advancedPuzzle.CheckDirections(word, x, y))
					{
						result = to_string(x) + " " + to_string(y) + " " + word;
						resultList.push_back(result);
					}
				}
			}
		}
	}
	for (i = resultList.begin(); i != resultList.end(); i++)
	{
		string word = *i;
		cout << word << endl;
	}

	popTimeTakenInSeconds = popTimeTakenInSecondsAdvanced;

	QueryPerformanceCounter(&end);
	timeTakenInSeconds = (end.QuadPart - start.QuadPart) / (double)(frequency.QuadPart*NUMBER_OF_RUNS);

	////////////////////////////////////////////////
	// This output should be to your results file //
	////////////////////////////////////////////////
	cout << fixed << setprecision(10) << "SolveAdvancedPuzzleWithSimpleDictionary() - " << timeTakenInSeconds << " seconds" << endl;
	cout << endl;
	return true;// change to true to write to file
}

bool WordSearch::SolveAdvancedPuzzleWithAdvancedDictionary() 
{
	//cout << endl << "SolveAdvancedPuzzleWithAdvancedDictionary() has NOT been implemented" << endl;
	cout << "AdvancedPuzzle + AdvancedDictionary" << endl;
	timeTakenInSeconds = 0;
	QueryPerformanceFrequency(&frequency);
	QueryPerformanceCounter(&start);
	vector<string>::iterator it;
	string word;
	int currentX;
	int currentY;
	int x;
	int y;
	int i;
	string result;

	AdvancedDictionary::CharNodes *startNode; 
	AdvancedDictionary::CharNodes *currentNode;

	for (int n = 0; n < NUMBER_OF_RUNS; ++n)
	{
		// Add your solving code here!
		resultList.clear();
		noOfVisitedCells = 0;
		noOfVisitedEntries = 0;
		for (int y = 0; y < advancedPuzzle.GetHeight(); y++)
		{
			for (int x = 0; x < advancedPuzzle.GetWidth(); x++)
			{
				noOfVisitedCells++;
				AdvancedPuzzle::Cell* currentCell = advancedPuzzle.GetCell(x, y);
				startNode = advancedDictionary.GetNode(NULL, currentCell->letter);
				noOfVisitedEntries++;
				if (startNode != NULL)
				{
					for (it = currentCell->directionString.begin(); it != currentCell->directionString.end(); it++)
					{

						word = "";
						word += currentCell->letter;//ADDDDED
						string directionWord = *it;
						currentNode = startNode;
						for (i = 1; i < directionWord.length(); i++)
						{
							currentNode = advancedDictionary.GetNode(currentNode, directionWord[i]);
							noOfVisitedEntries++;
							if (currentNode == NULL)
							{
								break;
							}
							word += currentNode->letter;
							if (currentNode->endLetter)
							{
								result = to_string(x) + " " + to_string(y) + " " + word;
								resultList.push_back(result);
							}
						}
					}
				}
			}
		}
	}
	for (it = resultList.begin(); it != resultList.end(); it++)
	{
		string word = *it;
		cout << word << endl;
	}

	popTimeTakenInSeconds = popTimeTakenInSecondsAdvanced;

	QueryPerformanceCounter(&end);
	timeTakenInSeconds = (end.QuadPart - start.QuadPart) / (double)(frequency.QuadPart*NUMBER_OF_RUNS);

	////////////////////////////////////////////////
	// This output should be to your results file //
	////////////////////////////////////////////////
	cout << fixed << setprecision(10) << "SolveAdvancedPuzzleWithAdvancedDictionary() - " << timeTakenInSeconds << " seconds" << endl;
	cout << endl;
	return true;// change to true to write to file

}

void WordSearch::WriteResults(const string &fileName) 
{
	//loop through resultslist and print to filename
	//cout << "WriteResults() has NOT been implemented" << endl;
	ofstream out(fileName);
	vector<string>::iterator i;
	out << "NUMBER_OF_WORDS_MATCHED " << to_string(resultList.size()) << endl;
	out << endl;
	out << "WORDS_MATCHED_IN_GRID" << endl;
	for (i = resultList.begin(); i != resultList.end(); i++)
	{
		string result = *i;
		out << result << endl;
	}
	out << endl;
	out << "NUMBER_OF_GRIDCELLS_VISTED " << noOfVisitedCells << endl;
	out << endl;
	out << "NUMBER_OF_DICTIONARY_ENTRIES_VISITED " << noOfVisitedEntries << endl;
	out << endl;
	out << "TIME_TO_POPULATE_GRID_STRUCTURE " << fixed << setprecision(10) << popTimeTakenInSeconds << " seconds" << endl;
	out << endl;
	out << "TIME_TO_SOLVE_PUZZLE " << timeTakenInSeconds << " seconds" << endl;
	out << endl;
	out.close();
}

		
	