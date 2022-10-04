#include "SimplePuzzle.h"


SimplePuzzle::SimplePuzzle() : gridWidth(0), gridHeight(0), puzzleGrid(NULL)
{

}

void SimplePuzzle::LoadPuzzle(const string &puzzlepath)
{
	ifstream file;
	file.open(puzzlepath);
	if (file.is_open())
	{
		gridHeight = 1;
		char letter;
		string puzzleText;
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
	else cout << "unable to load file - simplepuzzle";
}

void SimplePuzzle::CreatePuzzle(const string &text)
{
	puzzleGrid = new char*[gridWidth];
	for (int x = 0; x < gridWidth; x++)
	{
		puzzleGrid[x] = new char[gridHeight];
		for (int y = 0; y < gridHeight; y++)
			puzzleGrid[x][y] = text[y * gridWidth + x];
	}
}

void SimplePuzzle::PrintPuzzle() const
{
	for (int y = 0; y < gridHeight; y++)
	{
		for (int x = 0; x < gridWidth; x++)
		{
			cout << puzzleGrid[x][y];
		}
		cout << endl;
	}
}

int SimplePuzzle::GetHeight() const
{
	return gridHeight;
}

int SimplePuzzle::GetWidth() const
{
	return gridWidth;
}

char SimplePuzzle::GetChar(int x, int y) const
{
	if (puzzleGrid != NULL)
	{
		if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight)
		{
			return NULL;
		}
		else
			return puzzleGrid[x][y];
	}
}

SimplePuzzle::~SimplePuzzle()
{
	try
	{
		if (puzzleGrid != NULL)
		{
			for (int x = 0; x < gridWidth; x++)
			{
				delete[] puzzleGrid[x];
			}
			delete[] puzzleGrid;
			puzzleGrid = NULL;
		}
	}
	catch (exception &e)
	{
		cout << e.what() << endl;
	}
}
