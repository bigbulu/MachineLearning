import MySQLdb
conn = MySQLdb.connect(host="localhost",
                       user="root",
                       passwd="1234",
                       db="testdb")
x = conn.cursor()

try:
    x.execute("""INSERT INTO table1 VALUES (%s,%s)""", (1,1.23))
    conn.commit()
except:
    conn.rollback()

conn.close()