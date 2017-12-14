import MySQLdb
conn = MySQLdb.connect(host="localhost",
                       user="root",
                       passwd="1234",
                       db="testdb")
x = conn.cursor()

file = open("F:/DataMining/anonymized_dataset_for_ADM2017/training_label.csv", "r")
skip = True
for line in file:
    if (skip):
        skip = False
        continue
    data = line.split(',')
    try:
        x.execute("""INSERT INTO traininglabel VALUES (%s,%s,%s,%s,%s)""",
                      (data[0], data[1], data[2],
                       data[3], data[4]))
        conn.commit()
    except Exception as e:
        conn.rollback()
        #print(e)

conn.close()