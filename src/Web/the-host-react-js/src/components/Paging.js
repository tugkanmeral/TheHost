import { useState } from "react";

function Paging(props) {
    const [activePage, setActivePage] = useState(props.activePage);
    const [prevButtonVisible, setPrevButtonVisibility] = useState(false);
    const [nextButtonVisible, setNextButtonVisibility] = useState(true);

    let tempVisiblePage = []
    for (let index = 0; index < props.pageCount; index++) {
        tempVisiblePage.push(index + 1)
        if (index > 2)
            break
    }

    const [visiblePages, setVisiblePages] = useState(tempVisiblePage);

    function changePage(_page) {
        if (_page == activePage)
            return
            
        setActivePage(_page)
        props.onClick(_page)

        setDirectionButtonVisibility(_page);
    }

    function setDirectionButtonVisibility(_activePage) {
        if (_activePage <= 1) {
            setPrevButtonVisibility(false)
        }
        else {
            setPrevButtonVisibility(true)
        }

        let any = visiblePages.some(x => x == (_activePage + 1))
        if (any)
            setNextButtonVisibility(true)
        else
            setNextButtonVisibility(false)

    }

    function nextPage() {
        let any = visiblePages.some(x => x == (activePage + 1))
        console.log(any)
        if (any)
            changePage(activePage + 1)
    }

    function previousPage() {
        if (activePage <= 1)
            return

        changePage(activePage - 1)
    }

    function GetVisiblePages() {
        const pages = visiblePages.map((visiblePage, index) =>
            <li
                key={index}
                className={activePage === visiblePage ? 'page-item active' : 'page-item'}
            >
                <button className="page-link" onClick={() => changePage(visiblePage)}>{visiblePage}</button>
            </li>
        );

        return pages;
    }

    return (
        <div className="center-row">
            <ul className="pagination">
                <li className={prevButtonVisible ? "page-item" : "page-item disabled"}>
                    <button
                        className="page-link"
                        onClick={previousPage}
                        aria-label="Previous">
                        <span aria-hidden="true">&laquo;</span>
                    </button>
                </li>
                <GetVisiblePages />
                <li className={nextButtonVisible ? "page-item" : "page-item disabled"}>
                    <button
                        className="page-link"
                        onClick={nextPage}
                        aria-label="Next">
                        <span aria-hidden="true">&raquo;</span>
                    </button>
                </li>
            </ul>
        </div>
    )
}

export default Paging