## Problem 1

### Description:

The task involves predicting diabetes in patients based on their medical history and demographic information using the Bayesian classifier and decision tree classifier.
The dataset includes features such as age, gender, BMI, hypertension, heart disease, smoking history, HbA1c level, blood glucose level, with the class label being diabetes.
The goal is to build classifier models using the training set and evaluate their accuracy on the testing dataset.
Requirements:

Apply Bayesian and Decision Tree algorithms to build two classifier models using the training set.
Evaluate the accuracy of the classifiers by applying them to the testing dataset.
Compare the results of the Bayesian and Decision Tree classifiers.

## Problem 2

### Description:

This problem involves classifying potential customers who are more likely to purchase a loan using the Backpropagation Neural Network and k-Nearest-Neighbor Classifiers.
The dataset includes 5000 observations with features divided into different measurement categories: binary, interval, ordinal, and nominal. The target variable is whether the customer accepted the personal loan offered in the last campaign.
The main goal is to correctly identify potential loan customers.
Data Description:

Features include Customer ID, Age, Experience, Income, ZIP Code, Family size, CCAvg (Average spending on credit cards per month), Education level, Mortgage value, Securities Account, CD Account, Online banking usage, and Credit Card usage.
The target variable is Personal Loan (whether the customer accepted the loan or not).
Requirements:

Apply Backpropagation Neural Network and k-Nearest-Neighbor algorithms to build two classifier models using the training set.
Evaluate the accuracy of the classifiers on the testing dataset.
Compare the results of the Backpropagation Neural Network and k-Nearest-Neighbor classifiers.
Important Notes:

Preprocess the data to handle noise values, such as negative values for experience, by replacing them with their absolute values.
Drop features with high correlation (>0.98) to avoid degraded learning performance and instability in the models.