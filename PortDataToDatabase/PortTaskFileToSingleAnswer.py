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
                x.execute("""INSERT INTO problemanswered VALUES (
                %s,%s,%s,%s,%s,%s,%s,%s,%s,%s,
                %s,%s,%s,%s,%s,%s,%s,%s,%s,%s,
                %s,%s,%s,%s,%s,%s,%s,%s,%s,%s,
                %s,%s,%s,%s,%s,%s,%s,%s,%s,%s,
                %s,%s,%s,%s,%s,%s,%s,%s,%s,%s,
                %s,%s,%s,%s,%s,%s,%s,%s,%s,%s,
                %s,%s,%s,%s,%s)""",
                      (data[12], data[13], data[14], data[15], data[16],
                       data[17], data[18], data[19], data[20], data[21],
                       data[22], data[23], data[24], data[25], data[26],
                       data[27], data[28], data[29], data[30], data[31],
                       data[32], data[33], data[34], data[35], data[36],
                       data[37], data[38], data[39], data[40], data[41],
                       data[42], data[43], data[44], data[45], data[46],
                       data[47], data[48], data[49], data[50], data[51],
                       data[52], data[53], data[54], data[55], data[56],
                       data[57], data[58], data[59], data[60], data[61],
                       data[62], data[63], data[64], data[65], data[66],
                       data[67], data[68], data[69], data[70], data[71],
                       data[72], data[73], data[74], data[75].rstrip(), data[0]))
                conn.commit()
            except Exception as e:
                conn.rollback()
                #print(e)

conn.close()