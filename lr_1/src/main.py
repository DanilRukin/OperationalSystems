from configs import *
from packet_system import System
import sys
from matplotlib import pyplot as plt


if __name__ == "__main__":
    os = System()
    sessions = []
    sessions_count = input("Количество сессий: ")
    sessions_count = int(sessions_count)

    for session_index in range(sessions_count):
        variant = input("Выбирете вариант конфигурации (random - 1, только Process - 2, только IO - 3, IO = Process 4): ")
        variant = int(variant)
        if variant == 1:
            os.configure(random_config)
        elif variant == 2:
            os.configure(only_proccess_instructions_config)
        elif variant == 3:
            os.configure(only_io_instructions_config)
        elif variant == 4:
            os.configure(equal_count_of_io_and_process_instructions_in_package_config)

        productivity, working_time, processor_downtime = os.start(40)

        io_instructions_count = os.get_count_of_IO_instructions()
        process_instructions_count = os.get_count_of_Process_instructions()

        sessions.append((session_index, productivity, working_time, processor_downtime,
                          io_instructions_count, process_instructions_count))

        logger = os.logger
        logger.info(f"======================= сессия {session_index + 1} =======================")
        logger.info(f"Время работы системы: {os.last_session_work_time} с")
        logger.info(f"Производительность: {productivity}")
        logger.info(f"Оборотное время: {working_time} с")
        logger.info(f"Время простоя процессора: {processor_downtime} с")
        logger.info(f"Число задач с типом IO: {io_instructions_count} шт")
        logger.info(f"Число задач с типом Process: {process_instructions_count} шт")
        logger.info(f"======================= сессия {session_index + 1} (конец) ===============")
    
    plt.suptitle('Зависимость выходных параметров модели от соотношения типов решаемых задач')
    sessions = sorted(sessions, key=lambda session: session[4] / session[5])

    plt.subplot(1, 3, 1)
    plt.title('Производительность')
    plt.xlabel('Кол-во инструкций IO /\r\n Кол-во инструкций Process')
    plt.ylabel('Кол-во инструкций в секунду')
    plt.plot([x[4] / x[5] for x in sessions], [x[1] for x in sessions])

    plt.subplot(1, 3, 2)
    plt.title('Оборотное время')
    plt.xlabel('Кол-во инструкций IO /\r\n Кол-во инструкций Process')
    plt.ylabel('Время с')
    plt.plot([x[4] / x[5] for x in sessions], [x[2] for x in sessions])

    plt.subplot(1, 3, 3)
    plt.title('Время простоя процессора')
    plt.xlabel('Кол-во инструкций IO /\r\n Кол-во инструкций Process')
    plt.ylabel('Время с')
    plt.plot([x[4] / x[5] for x in sessions], [x[3] for x in sessions])
    plt.show()
