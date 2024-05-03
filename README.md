# Data Mining

this repo contains solution of data mining tasks and contians many algorithms that implemented from scratch.

## Problem 1

### Description:

The dataset "Bakery.csv" contains transaction data from a bakery where each row represents a transaction with multiple items purchased.
The task is to discover associations between items using association rule mining algorithms such as **Apriori**, FP-Growth, or vertical data format.
The program should allow the user to input minimum support and minimum confidence thresholds during runtime.
After mining, the program should generate frequent item sets and association rules with their confidence.
Requirements:

Write a program in any programming language implementing one of the association algorithms **(Apriori, FP-Growth, or vertical data format)** on the transaction dataset.
Allow user input for minimum support and minimum confidence.
Generate frequent item sets and association rules.
Display the frequent item sets and association rules with their confidence.

## Problem 2

### Description:

The dataset "Scores.xlsx" contains scores collected by school members in Tennis, Basketball, and Swimming.
Each row represents scores of one student, categorized into ranking, top level, and superior.
The task is to find associations between scores in one sport with reference to the scores in the other two sports.
For example, superior in swimming might correlate with top level in basketball and ranking in tennis.
The program should use one of the association algorithms **(Apriori, FP-Growth, or vertical data format)** to discover these associations.
User input for minimum support and minimum confidence thresholds should be allowed.
The final output should display frequent item sets and association rules with their confidence.

## Problem 3

### Description:

The dataset "imdb_top_2000_movies.csv" contains ratings of top 2000 movies between 1921 and 2010 scraped from IMDB's official website.
The task is to group the movies based on the similarity of user IMDB ratings using the **k-means** algorithm.
User input for the number of clusters (k) should be provided.
Initial centroids should be chosen randomly, and **Euclidean distance** should be used as the distance function.
Outlier detection should be implemented if necessary.
The final output should display k lists of movies grouped into clusters and show outlier movie records if any.

## Problem 4

### Description:

The dataset "Facebook_live.csv" contains statistics regarding different interactions in different types of Facebook posts.
The task is to implement k-means clustering to find intrinsic groups within the dataset based on status_type behavior.
Status_type behavior consists of posts of different natures like videos, photos, statuses, and links.
User input for the number of clusters (k) should be provided.
Initial centroids should be chosen randomly, and **Manhattan distance** should be used as the distance function.
Outlier detection should be implemented if necessary.
The final output should display k lists of states representing groups and show outlier post records if any.
