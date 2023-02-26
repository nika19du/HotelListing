import re

def convert_to_binary(text):
    integers = re.findall(r'\b\d+\b', text)
    for i in range(len(integers)):
        integers[i] = bin(int(integers[i]))[2:]

    for i in range(len(integers)):
        text = text.replace(str(i), integers[i],1)
    return text

text = "This text contains the numbers 5, 10 and 20, and also a number in hexadecimal 0x15"
print(convert_to_binary(text))
