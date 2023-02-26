import re
 
names = {
    'Мария': 'Maria',
    'Георги': 'George',
    'Йоан':'John',
    'Александър':'Alexander',
    'Никол':'Nicole',
    'Никола':'Nikola',
    'Юлиан':'Julian',
    'Катя':'Kate',
    'Надежда':'Hope',
    'Кристиан':'Christian',
    'Йордан':'Jordan',
    'Юлия':'Julia',
    'Виктор':'Victor',
    'Екатерина':'Catherine',
    'Николай':'Nicholas',
    'Йовановска':'Jovanovska'
}

pattern = r'[А-Я][а-я]*(?: [А-Я][а-я]*)*'

with open('input.txt', 'r', encoding='utf-8') as f:
    input_text = f.read()

output_text = re.sub(pattern, lambda m: names[m.group(0)], input_text)

with open('output.txt', 'w', encoding='utf-8') as f:
    f.write(output_text)