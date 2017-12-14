import MySQLdb
conn = MySQLdb.connect(host="localhost",
                       user="root",
                       passwd="1234",
                       db="testdb")
x = conn.cursor()

for i in range(1, 11):

        file = open("F:/DataMining/anonymized_dataset_for_ADM2017/student_log_{}.csv".format(i), "r")
        skip = True
        for line in file:
            if (skip):
                skip = False
                continue
            data = line.split(',')
            try:
                x.execute("""INSERT INTO testrecord VALUES (%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s)""",
                      (data[0], data[1], data[2],
                       data[3], data[4], data[5],
                       data[6], data[7], data[8],
                       data[9],data[10], data[11]))
                conn.commit()
            except Exception as e:
                conn.rollback()
                #print(e)

conn.close()