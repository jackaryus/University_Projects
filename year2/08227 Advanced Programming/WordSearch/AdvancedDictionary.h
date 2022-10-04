#pragma once

#include <algorithm>
#include <iterator>
#include <iostream>
#include <iomanip>
#include <fstream>
#include <string>
#include <vector>

using namespace std;

class AdvancedDictionary
{
public:
	AdvancedDictionary();
	~AdvancedDictionary();
	void LoadDictionary(const string &);	
	void PrintAdvancedDictionaryPublic();
	struct CharNodes
	{
		char letter;
		bool endLetter;
		vector<CharNodes*> children;
		~CharNodes()
		{
			try
			{
				vector<CharNodes*>::iterator i;
				for (i = children.begin(); i != children.end(); i++)
				{
					CharNodes* node = *i;
					delete node;
				}
				children.empty();
			}
			catch (exception e)
			{
				cout << e.what() << endl;
			}
		}
	};
	CharNodes* GetNode(CharNodes*, char);

private:
	vector<CharNodes*> dictionaryBaseNodeList;
	void PrintAdvancedDictionary(vector<CharNodes*> &, const string &);
	void AddToDictionary(vector<CharNodes*> &, string &, int);
};

