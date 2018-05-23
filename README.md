# Project Description

The purpose of this project is to create tools to assist the Music Department of the Haifa University in their research.
The researchers are analyzing manually written music sheets from the early 19th century from a synagogue in Copenhagen. The manuscript was written and rewritten for over an unknown length of time, and by several different people – sometimes interchangeably. The purpose of their research is to try and determine how many different writers participated in writing and rewriting the manuscript and over how long a period. We have been asked to help them create tools to assist with their research, specifically a system that could automatically distinguish between the different authors and help them assess how many different writers were involved.

# Initial Database Creation

Our project uses machine learning and computer vision to classify and categorize notes according to their authors. To achieve this, we first had to create an initial database of notes and their respective authors, so that the learning algorithms have something to learn from. This has to be done by one of the actual researchers, so to make the process easier for them and for prospective future researchers, we created a program called “Note Profiler”, which automatically detects and highlights notes on any scanned manuscript page, then lets the researcher choose which author wrote each note – which in the process also creates a database of profiled notes which we can later feed to our learning systems. Because the researchers are not necessarily as adept with computers in general and coding specifically, a lot of focus went into making this tool as user-friendly as possible, and we worked closely with Avital Vovnoboy to ensure everything was both functional and easy to use.

# Note Profiler Demo
[![Note Profiler Demo Video](https://img.youtube.com/vi/k-w9zZlEL3Y/0.jpg)](https://www.youtube.com/watch?v=k-w9zZlEL3Y)

# Machine Learning

Once we had an initial database of existing matches to work with, we went on to create the core of our project: the machine learnings system.
Written in Python, this system takes a training set from the aforementioned database (with a sample size that can be determined by the researcher), and uses that set to train and learn what characteristics apply to each of the authors. It does so by representing each and every note by a vector of attributes, and the values of these attributes, which are the key differences between the different authors, is how the program learns to approximate (to within a margin of error) which author wrote which note.

Finally, the program uses one of two algorithms to differentiate between the authors given the training set of the size specified by the researcher:

1. **SVM** – an unsupervised algorithm, meaning that it attaches a similarity index to each and every one of the authors but doesn’t make a final decision.
2. **Random Forest** – a supervised algorithm, meaning that the algorithm has to make a decision as to who is the most likely author of each note.
The application is interactive and user-friendly and accepts any set of notes in different sizes, which makes it useful both for researchers and for developers who want to use the same method to classify any written manuscript. The application output is an easy-to-read HTML file which displays the results in a clear and concise graphical representation.

# User and Developer Guide
For more details, see https://sites.hevra.haifa.ac.il/rbd/2017/09/06/music-manuscript-author-recognizer/

<br>

***ALL RIGHTS RESERVED TO THE HAIFA UNIVERSITY ROBOTICS & BIG DATA LAB***
