import pandas as pd
import numpy as np

class GaussianNaiveBayes:
    def fit(self, X_train, y_train):
        self.X_train = X_train
        self.y_train = y_train
        self.classes = np.unique(y_train)
        self.parameters = []

        for i, c in enumerate(self.classes):
            X_c = X_train[y_train == c]
            self.parameters.append([])
            for col in X_train.columns:
                mean = X_c[col].mean()
                std = X_c[col].std()
                self.parameters[i].append({'mean': mean, 'std': std})

    def _calculate_probability(self, x, mean, std):
        exponent = np.exp(-((x - mean) ** 2 / (2 * std ** 2)))
        return (1 / (np.sqrt(2 * np.pi) * std)) * exponent

    def _calculate_class_probabilities(self, row):
        probabilities = {}
        for i, c in enumerate(self.classes):
            probabilities[c] = 1
            for j, col in enumerate(self.X_train.columns):
                mean = self.parameters[i][j]['mean']
                std = self.parameters[i][j]['std']
                probabilities[c] *= self._calculate_probability(row[col], mean, std)
        return probabilities

    def predict(self, X_test):
        predictions = []
        for index, row in X_test.iterrows():
            probabilities = self._calculate_class_probabilities(row)
            best_label, best_prob = None, -1
            for class_value, probability in probabilities.items():
                if best_label is None or probability > best_prob:
                    best_prob = probability
                    best_label = class_value
            predictions.append(best_label)
        return predictions

    def accuracy(self, y_true, y_pred):
        correct = sum(y_true == y_pred)
        return correct / len(y_true)