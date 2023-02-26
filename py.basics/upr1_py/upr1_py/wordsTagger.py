import requests

url = "https://api.meaningcloud.com/topics-2.0"

payload={
    'key': 'ba0ec47c838e384fbc3642db021edd6e',
    'txt': '',
    'lang': 'bg',  # 2-letter code, like en es fr ...
    'tt': 'a'                      # all topics
}

with open('readme.txt', encoding='utf-8') as f:
	payload['txt']=str(f.readlines())


response = requests.post(url, data=payload)

#print('Status code:', response.status_code)
#print(response.json())

for obj in response.json()['concept_list']:
	print(f"Concept: form {obj['form']}, Type: {obj['sementity']['type']}")
	print(f"Relevance: {obj['relevance']}")

for obj in response.json()['entity_list']:
	print(f"Entity: {obj['form']}, link: {obj['semld_list'][0]}")

