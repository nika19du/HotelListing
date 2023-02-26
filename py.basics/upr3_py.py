# coding=utf8
# the above tag defines encoding for this document and is for Python 2.x compatibility

import re

regex = r"(\$|USD)*(\d)([\.,[\d+)?"

test_str = ("dawwwwwwwww $2.30\n\n"
	"$13.20 $3 USD 12.34\n"
	"USD 5000")

dict_currencies = {"EUR":0.94, "BGN":1.84, "BTC":0.000059}

user_currency=input("Select currency")

matches = re.finditer(regex, test_str, re.MULTILINE)
broj_celi=0
sum=0.0 


for matchNum, match in enumerate(matches, start=1):
    if match.group(3) is None:
        broj_celi+=1
        sum+=float(match.group(2))
    else:
        sum+=float(f"{match.group(2)}.{match.group(4)}")


    print ("Match {matchNum} was found at {start}-{end}: {match}".format(matchNum = matchNum, start = match.start(), end = match.end(), match = match.group()))
    
    for groupNum in range(0, len(match.groups())):
        groupNum = groupNum + 1
        
        print ("Group {groupNum} found at {start}-{end}: {group}".format(groupNum = groupNum, start = match.start(groupNum), end = match.end(groupNum), group = match.group(groupNum)))

print(f"sum={sum}")
set1={"aaa",1,2.3} #set unique values
constList={"aa","bbb",3} #constant list with constant values
list1=["ssg","sddsds","aaa",22,3]

# is instance dali dadeno nejo e konkreten tip
[print(x)for x in list1 if isinstance(x,str)and x.islower()]
try:
    convValue=sum*dict_currencies[user_currency]
    print(f"USD {sum} = {user_currency} {convValue}")
except KeyError as err:
    print(f"Not such value {KeyError}")