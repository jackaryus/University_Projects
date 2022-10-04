#include "AdvancedDictionary.h"

AdvancedDictionary::AdvancedDictionary()
{
}

void AdvancedDictionary::LoadDictionary(const string &dictionary)
{
	ifstream file;
	file.open(dictionary);
	if (file.is_open())
	{
		string line;
		while (getline(file, line))
		{
			AddToDictionary(dictionaryBaseNodeList , line, 0);
		}
		
	}
}

void AdvancedDictionary::AddToDictionary(vector<CharNodes*> &List, string &word, int wordIt)
{
	vector<CharNodes*>::iterator i;
	bool rootNodeFound = false;
	for (i = List.begin(); i != List.end(); i++)
	{
		CharNodes* currentNode = *i;
		if (word[wordIt] == currentNode->letter)
		{
			rootNodeFound = true;
			if (wordIt == word.length() - 1)
			{
				if (!currentNode->endLetter)
					currentNode->endLetter = true;
			}
			else
				AddToDictionary(currentNode->children, word, wordIt + 1);
			break;
		}
	}

	if (!rootNodeFound)
	{
		CharNodes* currentWord = new CharNodes;
		currentWord->letter = word[wordIt];
		if (wordIt == word.length() - 1)
		{
			currentWord->endLetter = true;
		}
		else
			currentWord->endLetter = false;
		List.push_back(currentWord);
		if (!currentWord->endLetter)
			AddToDictionary(currentWord->children,word, wordIt + 1);
	}
}

void AdvancedDictionary::PrintAdvancedDictionary(vector<CharNodes*> &List, const string &preString)
{
	vector<CharNodes*>::iterator i;
	for (i = List.begin(); i != List.end(); i++)
	{
		CharNodes* currentWord = *i;
		string dictionaryWord = preString;
		dictionaryWord += currentWord->letter;
		if (currentWord->endLetter)
		{
			cout << dictionaryWord << endl;
			dictionaryWord += "->";
		}
		PrintAdvancedDictionary(currentWord->children, dictionaryWord);
	
	}
}

void AdvancedDictionary::PrintAdvancedDictionaryPublic()
{
	PrintAdvancedDictionary(dictionaryBaseNodeList, "");
}

AdvancedDictionary::CharNodes* AdvancedDictionary::GetNode(CharNodes* node , char searchChar)
{
	vector<CharNodes*>::iterator i;
	if (node == NULL)
	{	
		for (i = dictionaryBaseNodeList.begin(); i != dictionaryBaseNodeList.end(); i++)
		{
			CharNodes* currentNode = *i;
			if (currentNode->letter == searchChar)
			{
				return currentNode;
			}
		}
	}
	else
	{
		for (i = node->children.begin(); i != node->children.end(); i++)
		{
			CharNodes* currentNode = *i;
			if (currentNode->letter == searchChar)
			{
				return currentNode;
			}
		}
	}
	return NULL;
}

AdvancedDictionary::~AdvancedDictionary()
{
	try
	{
		vector<CharNodes*>::iterator i;
		for (i = dictionaryBaseNodeList.begin(); i != dictionaryBaseNodeList.end(); i++)
		{
			CharNodes* node = *i;
			delete node;
		}
		dictionaryBaseNodeList.empty();
	}
	catch (exception &e)
	{
		cout << e.what() << endl;
	}
}
