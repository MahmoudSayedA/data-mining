import pandas as pd
import numpy as np
from collections import Counter
import math
from sklearn.model_selection import train_test_split

class DecisionTree:
    def __init__(self):
        self.tree = None

    def fit(self, X, y):
        # train the decision tree model
        self.tree = self._grow_tree(X, y)

    def _entropy(self, y):
        # calculate entropy given a set of labels
        counter = Counter(y)
        entropy = 0.0
        for label in counter:
            prob = counter[label] / len(y)
            entropy -= prob * math.log(prob, 2)
        return entropy

    def _information_gain(self, X, y, feature, threshold):
        # calculate information gain for a specific feature and threshold
        left_indices = X[feature] <= threshold
        right_indices = X[feature] > threshold

        left_y = y[left_indices]
        right_y = y[right_indices]

        left_entropy = self._entropy(left_y)
        right_entropy = self._entropy(right_y)

        total_instances = len(y)
        left_weight = len(left_y) / total_instances
        right_weight = len(right_y) / total_instances

        return self._entropy(y) - (left_weight * left_entropy + right_weight * right_entropy)

    def _best_split(self, X, y):
        # Method to find the best feature and threshold to split the data
        best_gain = 0.0
        best_feature = None
        best_threshold = None

        for feature in X.columns:
            thresholds = sorted(set(X[feature]))

            for threshold in thresholds:
                gain = self._information_gain(X, y, feature, threshold)
                if gain > best_gain:
                    best_gain = gain
                    best_feature = feature
                    best_threshold = threshold

        return best_feature, best_threshold

    def _grow_tree(self, X, y, depth=0, max_depth=10):
        # Method to recursively grow the decision tree
        if depth >= max_depth or len(set(y)) == 1:
            return Counter(y).most_common(1)[0][0]

        best_feature, best_threshold = self._best_split(X, y)
        if best_feature is None:
            return Counter(y).most_common(1)[0][0]

        left_indices = X[best_feature] <= best_threshold
        right_indices = X[best_feature] > best_threshold

        # Recursively grow left and right subtrees
        left_subtree = self._grow_tree(X[left_indices], y[left_indices], depth + 1, max_depth)
        right_subtree = self._grow_tree(X[right_indices], y[right_indices], depth + 1, max_depth)

        return (best_feature, best_threshold, left_subtree, right_subtree)

    def predict_instance(self, instance):
        # Method to predict the label of a single instance
        tree = self.tree
        while isinstance(tree, tuple):
            feature, threshold, left_subtree, right_subtree = tree
            # Traverse the tree based on feature values and thresholds
            if instance[feature] <= int(threshold):
                tree = left_subtree
            else:
                tree = right_subtree
        return tree

    def predict(self, X):
        # Method to predict the labels for multiple instances
        return [self.predict_instance(instance) for _, instance in X.iterrows()]

    def accuracy(self, y_true, y_pred):
            # Calculate the accuracy of the model.
            if len(y_true) != len(y_pred):
                raise ValueError("The number of true labels and predicted labels must be the same.")

            # Calculate the number of correct predictions
            correct_predictions = sum(1 for true, pred in zip(y_true, y_pred) if true == pred)

            # Calculate the total number of instances
            total_instances = len(y_true)

            # Calculate accuracy
            accuracy = correct_predictions / total_instances

            return accuracy
