from itertools import combinations

# Function to generate candidate itemsets of size k
def generate_candidates(itemsets, k):
    candidates = set()
    for itemset in itemsets:
        for item in itemset:
            candidates.add(frozenset(item))
    return list(combinations(candidates, k))

# Function to prune candidate itemsets using the Apriori property
def prune_candidates(itemsets, frequent_itemsets, k):
    pruned_candidates = []
    for candidate in itemsets:
        subsets = combinations(candidate, k - 1)
        if all(subset in frequent_itemsets for subset in subsets):
            pruned_candidates.append(candidate)
    return pruned_candidates

# Function to check if one set is a subset of another
def is_subset(candidate, transaction):
    candidate_set = set(candidate)
    transaction_set = set(transaction)
    for item in candidate_set:
        if item not in transaction_set:
            return True
    return True

# Function to calculate support for each candidate itemset
def calculate_support(transactions, candidates):
    support_counts = {}
    for transaction in transactions:
        for candidate in candidates:
            # if isinstance(candidate, tuple):
            #     candidate = frozenset(candidate)
            if is_subset(candidate, transaction):
                support_counts[candidate] = support_counts.get(candidate, 0) + 1
    num_transactions = len(transactions)
    support = {itemset: count / num_transactions for itemset, count in support_counts.items()}
    return support

# Function to calculate confidence for association rules
def calculate_confidence(frequent_itemsets, association_rules):
    confidence = {}
    for rule in association_rules:
        antecedent, consequent = rule
        support_antecedent = frequent_itemsets[antecedent]
        support_rule = frequent_itemsets[antecedent.union(consequent)]
        confidence[rule] = support_rule / support_antecedent
    return confidence

# Function to perform Apriori algorithm
def apriori(transactions, min_support, min_confidence):
    itemsets = [{frozenset([item])} for item in set.union(*transactions)]
    frequent_itemsets = {}
    k = 1
    while itemsets:
        candidates = generate_candidates(itemsets, k)
        support = calculate_support(transactions, candidates)
        frequent_itemsets_k = {itemset: sup for itemset, sup in support.items() if sup >= min_support}
        if not frequent_itemsets_k:
            break
        frequent_itemsets.update(frequent_itemsets_k)
        #itemsets = prune_candidates(candidates, frequent_itemsets_k.keys(), k)
        itemsets = frequent_itemsets
        k += 1
    
    association_rules = generate_association_rules(frequent_itemsets, min_confidence)
    
    return frequent_itemsets, association_rules

# Function to generate association rules from frequent itemsets
def generate_association_rules(frequent_itemsets, min_confidence):
    association_rules = []
    for itemset in frequent_itemsets.keys():
        if len(itemset) > 1:
            for i in range(1, len(itemset)):
                antecedents = list(combinations(itemset, i))
                for antecedent in antecedents:
                    consequent = itemset.difference(antecedent) # tuble
                    rule = (frozenset(antecedent), consequent)
                    association_rules.append(rule)
    return [rule for rule in association_rules if calculate_confidence(frequent_itemsets, [rule])[rule] >= min_confidence]

# Function to get user input for minimum support and confidence
def get_user_input():
    min_support = 0.5 #float(input("Enter the minimum support (between 0 and 1): "))
    min_confidence = 0.5  #float(input("Enter the minimum confidence (between 0 and 1): "))
    return min_support, min_confidence

# Example usage:
transactions = [
    {'Bread', 'Milk', 'Eggs'},
    {'Bread', 'Butter'},
    {'Milk', 'Eggs', 'Butter'},
    {'Bread', 'Milk', 'Eggs', 'Butter'},
]
min_support, min_confidence = get_user_input()
frequent_itemsets, association_rules = apriori(transactions, min_support, min_confidence)
print("Frequent Itemsets:")
print(frequent_itemsets)
print("Association Rules:")
print(association_rules)