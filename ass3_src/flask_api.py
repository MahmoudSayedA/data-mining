import os
import pandas as pd
from flask import Flask, render_template, jsonify, request
from sklearn.model_selection import train_test_split
from DecisionTree import DecisionTree
from NaiveBayes import GaussianNaiveBayes

app = Flask(__name__)

# Define the path to your CSV file
CSV_FILE_PATH = os.path.join(app.root_path, 'data', 'diabetes_prediction_dataset.csv')

@app.route('/')
def index():
    return render_template('index.html')

@app.route('/train', methods=['POST'])
def train_model():
    # Get the number of records from the request
    record_count = int(request.form['record-count'])

    # Read data from CSV file
    df = pd.read_csv(CSV_FILE_PATH).head(record_count)

    # Separate features and target variable
    X = df.drop(columns=['diabetes'])
    y = df['diabetes']

    # Split the data into training and testing sets (75% train, 25% test)
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.25, random_state=13)
    data_dict = X_test.copy() # as raw data

    # Convert categorical features to numerical
    gender_map = {"Female": 0, "Male": 1}
    smoking_history_map = {"never": 0, "current": 1, "not current": 2, "ever": 3, "former": 4, "No Info": 5}
    X_train['gender'] = X_train['gender'].map(gender_map)
    X_train['smoking_history'] = X_train['smoking_history'].map(smoking_history_map)
    X_test['gender'] = X_test['gender'].map(gender_map)
    X_test['smoking_history'] = X_test['smoking_history'].map(smoking_history_map)

    numerical_columns = ['age', 'bmi', 'HbA1c_level', 'blood_glucose_level']
    for column in numerical_columns:
        median_value = X_train[column].astype(float).median()
        median_value = X_test[column].astype(float).median()
        X_train[column] = (X_train[column].astype(float) >= median_value).astype(int)
        X_test[column] = (X_test[column].astype(float) >= median_value).astype(int)

    # Initialize model
    decision_tree_model = DecisionTree()
    naive_bayes_model = GaussianNaiveBayes()

    # Train the model
    decision_tree_model.fit(X_train, y_train)
    naive_bayes_model.fit(X_train, y_train)

    # Sample prediction
    decision_tree_predictions = decision_tree_model.predict(X_test)
    decision_tree_accuracy = decision_tree_model.accuracy(y_test, decision_tree_predictions)

    naive_bayes_predictions = naive_bayes_model.predict(X_test)
    naive_bayes_accuracy = naive_bayes_model.accuracy(y_test, naive_bayes_predictions)

    # Add the predictions to the dictionary
    data_dict['predicted_diabetes_DT'] = decision_tree_predictions
    data_dict['predicted_diabetes_NB'] = naive_bayes_predictions

    # Convert DataFrame to dictionary
    data_dict:dict = data_dict.to_dict(orient='records')

    # Return the dictionary as JSON response
    return jsonify({'decision_tree_accuracy': decision_tree_accuracy, 'naive_bayes_accuracy': naive_bayes_accuracy, 'data': data_dict})


if __name__ == '__main__':
    app.run(debug=True)

