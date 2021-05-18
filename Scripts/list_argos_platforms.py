
# -*- coding: utf-8 -*-
"""
Sends a request to the Argos Server to get a list of all the Argos Platforms
(IDs) for a given program.

The username and passwords for the programs can be found in the ArgosPrograms
table in the Animal Movements database

Third party requirements:
* requests - https://pypi.python.org/pypi/requests
"""

from __future__ import absolute_import, division, print_function, unicode_literals

import requests

template = """
<soap:Envelope xmlns:soap="http://www.w3.org/2003/05/soap-envelope" xmlns:argos="http://service.dataxmldistribution.argos.cls.fr/types">
<soap:Header/>
<soap:Body>
<argos:platformListRequest>
<argos:username>{0}</argos:username>
<argos:password>{1}</argos:password>
</argos:platformListRequest>
</soap:Body>
</soap:Envelope>"""

username = "XXX"
password = "sekrit"

request = template.format(username, password)

url = "http://ws-argos.clsamerica.com/argosDws/services/DixService"

resp = requests.post(url, data=request)

# TODO if the response is not a 200 status, then print the error and quit

print(resp.text)

# TODO Parse the XML response and print just the list of platformIds
