from package import *
from system import System
from matplotlib import pyplot as plt
import numpy as np

if __name__ == "__main__":
    sys = System([])
    sys.start()
    sys.stop()
    time = sys.get_current_time_ms()
    x = np.arange(0, time, 1)
    y = x

    plt.plot(x, y)
    plt.show()
