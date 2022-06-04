import tkinter as tk

from tkinter import Canvas


app = tk.Tk()
app.title("Canvas")

canvas = Canvas(app, width=1000, height=1000)
canvas.pack()

xs = (230, 460, 310, 540)  # 80 units
s = (335, 455, 425, 545)  # 90 units
m = (450, 450, 550, 550)  # 100 units
l = (565, 445, 675, 555)  # 110 units
xl = (690, 440, 810, 560)  # 120 units

canvas.create_oval(xs, fill="black")
canvas.create_oval(s, fill="black")
canvas.create_oval(m, fill="black")
canvas.create_oval(l, fill="black")
canvas.create_oval(xl, fill="black")

app.after(1000, app.destroy)
app.mainloop()

app = tk.Tk()
app.title("Canvas")
canvas = Canvas(app, width=1000, height=1000)
canvas.pack()

canvas.create_oval(450, 50, 550, 150, fill="black")
canvas.create_oval(450, 850, 550, 950, fill="black")

app.after(1000, app.destroy)
app.mainloop()

app = tk.Tk()
app.title("Canvas")
canvas = Canvas(app, width=1000, height=1000)
canvas.pack()

canvas.create_oval(50, 450, 150, 550, fill="black")
canvas.create_oval(850, 450, 950, 550, fill="black")

app.after(1000, app.destroy)
app.mainloop()


