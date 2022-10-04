micro_watson:- repeat,
    write('MicroWatson:'),nl,
	write('1. Novels/Films of the 20th Century'),nl,
	write('2. Novels of the 19th Century'),nl,
	write('3. Novels of the 18th Century'),nl,
	write('4. Exit'),nl,
read(Choice), Choice>0, Choice=<4,
doit(Choice).

doit(1):-
	write('Please give me an answer.'),nl,
	write('Micro_Watson:'),	read(Question), nl,
	question(Question, question(np(Noun_Phrase), vp(Verb_Phrase)), VPLIST),
	write('sentence('),nl,
	write('      '), write(Noun_Phrase), nl,
	write('      '), write(Verb_Phrase), write(')'), nl,
	parse_sov(Question, VPLIST, SOV),nl,
	/*write(SOV), nl, nl,*/
	best_matchstart(SOV),
	micro_watson.
doit(2):-
	nl,
	write('+------------------------------------------------+'), nl,
	write('¦Novels of the 19th Century is under construction¦'),nl,
	write('+------------------------------------------------+'), nl,
	write('			 ¦'), nl,
	write('^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^'), nl,
	micro_watson.
doit(3):-
	nl,
	write('+------------------------------------------------+'), nl,
	write('¦Novels of the 18th Century is under construction¦'),nl,
	write('+------------------------------------------------+'), nl,
	write('			 ¦'), nl,
	write('^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^'), nl,
	micro_watson.
doit(4):-
	nl, !.

question(Question, question(np(Noun_Phrase), vp(Verb_Phrase)), Rem):-
	np(Question, Noun_Phrase, Rem),
	vp(Rem, Verb_Phrase).

np([H|T], noun_phrase(det(H), NP2), Rem):-
	det(H),
	np2(T, NP2, Rem).
np(Question, Parse, Rem):-
	np2(Question, Parse, Rem).
np(Question, noun_phrase(NP, PP), Rem):-
	np(Question, NP , Rem1),
	pp(Rem1 , PP, Rem).

np2([H|T], np(noun(H)), T):-
	noun(H).
np2([H|T], np2(adj(H), Rest), Rem):-
	adj(H),
	np2(T, Rest, Rem).

pp([H|T],pp(prep(H), Parse), Rem):-
	prep(H),
	np(T, Parse, Rem).

vp([H|[]],verb(H)):-
	verb(H).
vp([H,A|Rest],verb_phrase((verb(H),adverb(A)),RestParsed)):-
	verb(H),
	adverb(A),
	np(Rest,RestParsed,_).
vp([H,A|[]],verb_phrase(verb(H),adverb(A))):-
	verb(H),
	adverb(A).
vp([H|Rest],verb_phrase(verb(H),RestParsed)):-
	verb(H),
	np(Rest,RestParsed,_).
vp([H|Rest],verb_phrase(verb(H),RestParsed)):-
	vp(H, RestParsed),
	pp(Rest, RestParsed, _).

parse_sov(Question, VP_List ,TargetList):-
	/* Grab the first noun from the sentence and assume it is the subject */
	member(Subject,Question),
	noun(Subject),
	/* Grab the first noun from the verb phrase and assume it is the object */
	member(Object,VP_List),
	noun(Object),
	/* Grab the verb from the verb phrase */
	member(Verb,VP_List),
	verb(Verb),
	TargetList = [subject(Subject),object(Object),verb(Verb)].

pastchecks(X,[H|T],[H|NewT]):-
    pastchecks(X,T,NewT).
pastchecks(X,[],[X]).

bestcheck(X,XName,Y,_,X,XName):-X>Y.
bestcheck(_,_,    Y,YName,Y,YName).

best_matchstart(SOV_List):-
    write('Matches'),nl,
    best_match(SOV_List, [], "", 0).
best_match(SOV_List,Past_Searchs,Best_Name,Best_Count):-
	plot_story(Plot_List,Plot_Name),
	not(member(Plot_Name,Past_Searchs)),
	match(SOV_List,Plot_List,0,N),
	pastchecks(Plot_Name,Past_Searchs,Out),
	length(SOV_List,L),
	N > L/2,
	write(Plot_Name),write('    '),write(N),write('/'),write(L), nl,
	bestcheck(N,Plot_Name,Best_Count,Best_Name,Best_Count1,Best_Name1),
	best_match(SOV_List,Out,Best_Name1,Best_Count1).
best_match(SOV_List,_,Best_Name,Best_Count):-
	length(SOV_List,L),
	Best_Count > L/2,
	nl, write('Best Match'), nl,
	write(Best_Name), nl, nl,
    write('Give me a plot element from the following book '), write(Best_Name), nl, nl.
best_match(_,_,_,_).

/* The match predicate gives the number of atoms in the
 *   first list which are also present in the second list */
match([],_,N,N).
/* If the head of the list is a member of the history list
 * increment the counter and recurse on the tail.*/
match([H|T],List2,N,M):-
	member(H,List2),
	N1 is N +1,
	match(T,List2,N1,M).
/* Otherwise just recurse on the tail */
match([_|T],List2,N,M):-
	match(T,List2,N,M).



det(a).
det(the).

adj(young).
adj(middle_aged).
adj(magic).
adj(faithful).
adj(paranoid).

prep(on).
prep(by).

verb(finds).
verb(saves).
verb(is).
verb(destroys).
verb(was).

noun(expelled).

noun(hobbit).
noun(ring).
noun(valet).
noun(day).
noun(robot).
noun(marvin).
noun(holden).

adverb(quietly).

plot_story([subject(hobbit),object(ring),verb(finds)], 'The Hobbit').
plot_story([subject(hobbit),object(ring),verb(destroys)], 'The Lord of the Rings').
plot_story([subject(valet),object(day),verb(saves)], 'Thankyou Jeeves').
plot_story([subject(holden),object(expelled),verb(is)] , 'The Catcher in the Rye').
plot_story([subject(robot),object(marvin),verb(was)], 'The Hitchhikers Guide to the Galaxy').
