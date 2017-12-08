import numpy
import matplotlib.pyplot as plt
list1 = [34.7, 36.0, 29.3, 40.1, 35.7, 42.4, 40.3, 37.3, 40.9, 38.3, 38.5, 41.4, 39.7, 39.7, 41.1, 38.0, 38.7]
list2 = [1895, 2030, 1440, 2835, 3090, 3827, 3260, 2690, 3285, 2920, 3430, 3657, 3685, 3345, 3260, 2680, 2005]

print(numpy.corrcoef(list1, list2)[0, 1])
plt.plot(list1, list2, 'o')
plt.show()
