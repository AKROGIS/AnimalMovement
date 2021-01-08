Hints
=====

To convert reStructuredText to HTML and many other formats, you will need python doc utils

see http://docutils.sourceforge.net/ for download and installation instructions

To convert reStructuredText to PDF you will need rst2pdf (http://rst2pdf.ralsina.com.ar/)

to get rst2pdf, first install python setuptools (http://pypi.python.org/pypi/setuptools).
download ez_setup.py from that page and run as:

   C:\Python27\ArcGIS10.2\Python.exe ez_setup.py install

Then you can install all manner or python add-ins like:

   C:\Python27\ArcGIS10.2\Scripts\easy_install.exe rst2pdf

To embed images in the pdf documents, you will also need the Python Image Library (http://www.pythonware.com/products/pil/)::

   C:\Python27\ArcGIS10.1\Scripts\easy_install.exe pil


## Make HTML

```bat
c:\Python27\ArcGIS10.2\python.exe "C:\Users\resarwas\My Apps\docutils\tools\rst2html.py" "C:\Users\resarwas\Documents\GitHub\AnimalMovement\Documentation\UserGuide.rst" "C:\Users\resarwas\Documents\GitHub\AnimalMovement\Documentation\Documentation.html"
c:\Python27\ArcGIS10.2\python.exe "C:\Users\resarwas\My Apps\docutils\tools\rst2html.py" "C:\Users\resarwas\Documents\GitHub\AnimalMovement\Documentation\Installation Instructions.rst" "C:\Users\resarwas\Documents\GitHub\AnimalMovement\Documentation\Installation Instructions.html"
c:\Python27\ArcGIS10.2\python.exe "C:\Users\resarwas\My Apps\docutils\tools\rst2html.py" "C:\Users\resarwas\Documents\GitHub\AnimalMovement\Documentation\Build_Instructions.rst" "C:\Users\resarwas\Documents\GitHub\AnimalMovement\Documentation\Build_Instructions.html"
```

## Make PDF

```bat
"C:\Python27\ArcGIS10.2\Scripts\rst2pdf.exe" --default-dpi=100 "C:\Users\resarwas\Documents\GitHub\AnimalMovement\Documentation\UserGuide.rst" "C:\Users\resarwas\Documents\GitHub\AnimalMovement\Documentation\Documentation.pdf"
"C:\Python27\ArcGIS10.2\Scripts\rst2pdf.exe" --default-dpi=100 "C:\Users\resarwas\Documents\GitHub\AnimalMovement\Documentation\Installation Instructions.rst" "C:\Users\resarwas\Documents\GitHub\AnimalMovement\Documentation\Installation Instructions.pdf"
"C:\Python27\ArcGIS10.2\Scripts\rst2pdf.exe" --default-dpi=100 "C:\Users\resarwas\Documents\GitHub\AnimalMovement\Documentation\Build_Instructions.rst" "C:\Users\resarwas\Documents\GitHub\AnimalMovement\Documentation\Build_Instructions.pdf"
```

[Quick reStructuredText](http://docutils.sourceforge.net/docs/user/rst/quickref.html)

[reStructuredText Tool Support](http://stackoverflow.com/questions/2746692/restructuredtext-tool-support)

[rst2pdf Manual](http://lateral.netmanagers.com.ar/static/manual.pdf)
