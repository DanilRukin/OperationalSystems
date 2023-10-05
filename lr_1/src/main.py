from configs import *
from packet_system import System
import sys


if __name__ == "__main__":
    os = System()
    variant = input("Выбирете вариант конфигурации (1, 2, 3, 4): ")
    variant = int(variant)
    if variant == 1:
        os.configure(random_config)
    elif variant == 2:
        os.configure(only_proccess_instructions_config)
    elif variant == 3:
        os.configure(only_io_instructions_config)
    elif variant == 4:
        os.configure(equal_count_of_io_and_process_instructions_in_package_config)
    os.start(40)
    
