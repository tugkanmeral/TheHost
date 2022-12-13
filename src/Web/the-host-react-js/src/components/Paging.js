import { useState } from "react";

function Paging(props) {
    const [activePage, setActivePage] = useState(props.activePage);

    let tempVisiblePage = []
    for (let index = 0; index < props.pageCount; index++) {
        tempVisiblePage.push(index + 1)
        if (index > 2)
            break
    }
    
    const [visiblePages, setVisiblePages] = useState(tempVisiblePage);

    function changePage(_page) {
        setActivePage(_page)
        props.onClick(_page)
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
                <li className="page-item">
                    <a className="page-link" href="#" aria-label="Previous">
                        <span aria-hidden="true">&laquo;</span>
                    </a>
                </li>
                <GetVisiblePages />
                <li className="page-item">
                    <a className="page-link" href="#" aria-label="Next">
                        <span aria-hidden="true">&raquo;</span>
                    </a>
                </li>
            </ul>
        </div>
    )
}

export default Paging