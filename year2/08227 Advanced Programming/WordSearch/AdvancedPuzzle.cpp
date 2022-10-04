#include "AdvancedPuzzle.h"


AdvancedPuzzle::AdvancedPuzzle() : gridWidth(0), gridHeight(0), puzzleCells(NULL), puzzleText("")
{
}

void AdvancedPuzzle::LoadPuzzle(const string &puzzlepath)
{
	ifstream file;
	file.open(puzzlepath);
	if (file.is_open())
	{
		gridHeight = 1;
		char letter;
		while (file.get(letter))
		{
			if (letter == '\n')
			{
				gridHeight++;

			}
			else
			{
				if (letter != ' ')
				{
					puzzleText += letter;
				}
			}

		}
		file.close();
		gridWidth = puzzleText.length() / gridHeight;
		CreatePuzzle(puzzleText);
	}
	else cout << "unable to load file";
}

void AdvancedPuzzle::CreatePuzzle(string &text)
{
	for (int y = 0; y < gridHeight; y++)
	{
		for (int x = 0; x < gridWidth; x++)
		{
			Cell* currentCell = new Cell;
			currentCell->cellX = x;
			currentCell->cellY = y;
			currentCell->letter = text[y * gridWidth + x];
			for (int j = 0; j < 8; j++)
				currentCell->directionString.push_back(getDirectionStrings(text, j, x, y));
			puzzleCells.push_back(currentCell);
		}
	}
}

bool AdvancedPuzzle::CheckDirections(string &word, int cellx, int celly)
{
	bool found = false;
	Cell* cell = puzzleCells[celly * gridWidth + cellx];
	vector<string>::iterator i;

	for (i = cell->directionString.begin(); i != cell->directionString.end(); i++)
	{
		string directionString = *i;
		if (directionString.length() < word.length())
		{
			continue;
		}
		else
		{
			for (int i = 0; i < word.length(); i++)
			{
				if (word[i] != directionString[i])
					break;
				else if (i == word.length() - 1)
				{
					found = true;
				}
			}
		}
	}
	return found;
}

string AdvancedPuzzle::getDirectionStrings(string &text, int iterator, int cellx, int celly)const
{
	string directionString;
	switch (iterator)
	{
		case 0: //up
		{
			for (int y = celly; y >= 0; y--)
				directionString += text[y * gridWidth + cellx];
			return directionString;
		}
		break;

		case 1: //upright
		{
			int x = cellx;
			int y = celly;
			while (y >= 0 && x < gridWidth)
			{
				directionString += text[y * gridWidth + x];
				x++;
				y--;
			}
			return directionString;
		}
		break;

		case 2: //right
		{
			for (int x = cellx; x < gridWidth; x++)
				directionString += text[celly * gridWidth + x];
			return directionString;
		}
		break;

		case 3: //downright
		{
			int x = cellx;
			int y = celly;
			while (y < gridHeight && x < gridWidth)
			{
				directionString += text[y * gridWidth + x];
				x++;
				y++;
			}
			return directionString;
		}
		break;

		case 4: //down
		{
			for (int y = celly; y < gridHeight; y++)
				directionString += text[y * gridWidth + cellx];
			return directionString;
		}
		break;

		case 5: //downleft
		{
			int x = cellx;
			int y = celly;
			while (y < gridHeight && x >= 0)
			{
				directionString += text[y * gridWidth + x];
				x--;
				y++;
			}
			return directionString;
		}
		break;

		case 6: //left
		{
			for (int x = cellx; x >= 0; x--)
				directionString += text[celly * gridWidth + x];
			return directionString;
		}
		break;

		case 7: //upleft
		{
			int x = cellx;
			int y = celly;
			while (y >= 0 && x >= 0)
			{
				directionString += text[y * gridWidth + x];
				x--;
				y--;
			}
			return directionString;
		}
		break;
	}
}

int AdvancedPuzzle::GetHeight() const
{
	return gridHeight;
}

int AdvancedPuzzle::GetWidth() const
{
	return gridWidth;
}

AdvancedPuzzle::Cell* AdvancedPuzzle::GetCell(int x, int y)
{
	return puzzleCells[y * gridWidth + x];
}

AdvancedPuzzle::~AdvancedPuzzle()
{
	try
	{ 
		vector<Cell*>::iterator i;
		for (i = puzzleCells.begin(); i != puzzleCells.end(); i++)
		{
			Cell* cell = *i;
			delete cell;
		}
		puzzleCells.empty();
	}
	catch (exception &e)
	{
		cout << e.what() << endl;
	}
}
