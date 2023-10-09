from configs import *
from packet_system import System
import sys
from matplotlib import pyplot as plt
import logging


if __name__ == "__main__":
    GlobalParams.io_instruction_max_working_time = 4
    GlobalParams.io_instruction_min_working_time = 1
    GlobalParams.random_config_instructions_count = 20
    GlobalParams.random_config_packages_count = 7

    sessions = []
    sessions_count = input("Количество сессий: ")
    sessions_count = int(sessions_count)

    packet_os = System(get_logger())

    for session_index in range(sessions_count):     
        # variant = input("Выберете вариант конфигурации (random - 1, только Process - 2, только IO - 3, IO = Process 4): ")
        # variant = int(variant)
        variant = 1
        if variant == 1:
            packet_os.configure(random_config)
        elif variant == 2:
            packet_os.configure(only_proccess_instructions_config)
        elif variant == 3:
            packet_os.configure(only_io_instructions_config)
        elif variant == 4:
            packet_os.configure(equal_count_of_io_and_process_instructions_in_package_config)

        # число инструкций надо посчитать до старта системы, т.к. пакеты по мере выполнения выкидываются
        io_instructions_count = packet_os.get_count_of_IO_instructions()
        process_instructions_count = packet_os.get_count_of_Process_instructions()

        productivity, working_time, processor_downtime = packet_os.start(40)

        sessions.append((session_index, productivity, working_time, processor_downtime,
                          io_instructions_count, process_instructions_count))

        logger = packet_os.logger
        logger.info(f"======================= сессия {session_index + 1} =======================")
        logger.info(f"Время работы системы: {packet_os.last_session_work_time} с")
        logger.info(f"Производительность: {productivity}")
        logger.info(f"Оборотное время: {working_time} с")
        logger.info(f"Время простоя процессора: {processor_downtime} с")
        logger.info(f"Число задач с типом IO: {io_instructions_count} шт")
        logger.info(f"Число задач с типом Process: {process_instructions_count} шт")
        logger.info(f"======================= сессия {session_index + 1} (конец) ===============")
    
    logging.disable()

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

